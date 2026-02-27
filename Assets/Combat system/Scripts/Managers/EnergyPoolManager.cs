using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NueGames.NueDeck.Scripts.Card.CardActions.Energy;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Data.Containers;
using NueGames.NueDeck.Scripts.Data.Energy;
using NueGames.NueDeck.Scripts.Energy;
using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Managers
{
    public class EnergyPoolManager : MonoBehaviour
    {
        private EnergyPoolManager(){}
        public static EnergyPoolManager Instance {get; private set;}

        [Header("References")]
        [SerializeField] private List<Transform> energyPosList;
        [SerializeField] private List<EnergyData> availableEnergies;

        #region private fields
        private readonly float spawnTimer = 0.25f;
        #endregion

        #region Cache
        public List<EnergyBase> CurrentEnergyInPool {get; private set;} = new List<EnergyBase>();
        public List<Transform> EnergyPostList => energyPosList;
        private GameManager GameManager => GameManager.Instance;
        #endregion

        #region Setup
        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                Instance = this;
            }
        }
        
        #endregion

        #region Public methods
        public void CreateStartOfTurnEnergy()
        {
            EnemyEncounter currentEncounterData = GameManager.PersistentGameplayData.CurrentEncounter;

            List<EnergyQuantityData> energyCreationData = new();
            foreach(EnergyData data in currentEncounterData.AvailableEnergies)
            {
                int energyQuantity = UnityEngine.Random.Range(currentEncounterData.MinEnergySpawn, currentEncounterData.MaxEnergySpawn + 1); 
                energyCreationData.Add(new EnergyQuantityData(data.EnergyType, energyQuantity));
            }

            CreateEnergy(energyCreationData);
        }
        public void CreateEnergy(List<EnergyQuantityData> energyQuantityDataList)
        {
            StartCoroutine(CreateEnergyRoutine(energyQuantityDataList));
        }
        public void DecayAllEnergy()
        {
            for (int i = CurrentEnergyInPool.Count - 1; i >= 0; i--)
            {
                EnergyBase energy = CurrentEnergyInPool[i];

                EnergyStrength newStrength = EnergyStrengthHelper.GetNewEnergyStrengthValue(energy.EnergyStats.EnergyStrength,ModificationType.Weaken);

                energy.EnergyStats.ModifyStrength(newStrength);
            }
        }
        public void ConvertEnergy(List<EnergyConversion> energyToConvertList)
        {
            foreach(EnergyConversion energyConversionData in energyToConvertList)
            {
                TryToFindEnergyOnPool(new List<EnergyQuantityData> {energyConversionData.From}, out List<EnergyBase> energyToConvert);

                foreach(EnergyBase energy in energyToConvert)
                {
                    EnergyStrength energyStrength = energy.EnergyStats.EnergyStrength;
                    Transform spawnPos = energy.transform;
                    energy.OnDestroy();

                    EnergyData energyData = availableEnergies.FirstOrDefault(energy => energy.EnergyType == energyConversionData.To.EnergyColor);
                    EnergyBase clone = Instantiate(energyData.EnergyPrefab, spawnPos.position, spawnPos.rotation);
                    clone.transform.SetParent(spawnPos.parent, true);
                    
                    clone.BuildEnergy(energyStrength);
                    CurrentEnergyInPool.Add(clone);
                }
            }
        }
        public void ModifyEnergyStrength(List<EnergyStrengthModification> energyToModifyStrength)
        {
            foreach(EnergyStrengthModification energyModificationData in energyToModifyStrength)
            {
                TryToFindEnergyOnPool(new List<EnergyQuantityData> {energyModificationData.From}, out List<EnergyBase> energyToModify);

                foreach(EnergyBase energy in energyToModify)
                {
                    energy.EnergyStats.ModifyStrength(energyModificationData.To);
                }
            }
        }
        public void ConsumeEnergyCost(List<EnergyQuantityData> cost)
        {
            TryToFindEnergyOnPool(cost, out List<EnergyBase> targetEnergies);
            
            foreach(EnergyBase energy in targetEnergies)
            {
                energy.OnDestroy();
            }
        }
        public void RemoveEnergyFromPool(EnergyBase targetEnergy)
        {
            CurrentEnergyInPool.Remove(targetEnergy);
        } 

        public bool TryToFindEnergyOnPool(List<EnergyQuantityData> energiesToFind, out List<EnergyBase> selectedEnergies)
        {
            selectedEnergies = new();

            if(energiesToFind == null || energiesToFind.Count == 0)
                return true;

            foreach(EnergyQuantityData energyData in energiesToFind)
            {
                List<EnergyBase> foundEnergies = CurrentEnergyInPool.
                    Where(energy => energy.EnergyStats.EnergyColor == energyData.EnergyColor)
                    .OrderBy(energy => energy.EnergyStats.EnergyStrength)
                    .Take(energyData.Quantity)
                    .ToList();
                
                if(foundEnergies.Count < energyData.Quantity)
                {
                    selectedEnergies.Clear();
                    return false;
                }
                    
                selectedEnergies.AddRange(foundEnergies);
            }
            return true;
        }
        #endregion

        #region Routines
        private IEnumerator CreateEnergyRoutine(List<EnergyQuantityData> energyQuantityDataList)
        {
            foreach(EnergyQuantityData data in energyQuantityDataList)
            {
                for(int i=0; i < data.Quantity; i++)
                {
                    EnergyData energyData = availableEnergies.FirstOrDefault(energy => energy.EnergyType == data.EnergyColor);
                    int spawnPosition = UnityEngine.Random.Range(0, energyPosList.Count);
                    EnergyBase clone = Instantiate(energyData.EnergyPrefab, energyPosList[spawnPosition]);
                    clone.BuildEnergy();
                    CurrentEnergyInPool.Add(clone);
                    yield return new WaitForSeconds(spawnTimer);
                }
            }
        }
        #endregion
    }
}

