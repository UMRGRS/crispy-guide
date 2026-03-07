using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            EnergyGenerationParameters modifiedEnergyGenerationParameters = GameManager.PersistentGameplayData.EnergyModificationRules;

            if(modifiedEnergyGenerationParameters is not null && modifiedEnergyGenerationParameters.turns-- > 0)
            {
                CreateEnergy(DetermineEnergiesToCreate(modifiedEnergyGenerationParameters));
            }
            else
            {
                CreateEnergy(DetermineEnergiesToCreate(
                    new EnergyGenerationParameters(
                        0, 
                        currentEncounterData.MaxEnergySpawn, 
                        currentEncounterData.MaxEnergySpawn, 
                        currentEncounterData.AvailableEnergies)
                        ));
            }
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

                energy.EnergyStats.ModifyStrength(EnergyModificationType.Weaken);
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

                    EnergyData energyData = availableEnergies.FirstOrDefault(energy => energy.EnergyColor == energyConversionData.To.EnergyColor);
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
                    energy.EnergyStats.ModifyStrength(energyModificationData.ModificationType);
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

        #region Private methods
        private List<EnergyQuantityData> DetermineEnergiesToCreate(EnergyGenerationParameters energyGenerationRules)
        {
            Dictionary<EnergyColor, int> totals = new();

            int energyQuantity = UnityEngine.Random.Range(energyGenerationRules.minEnergiesSpawn, energyGenerationRules.maxEnergiesSpawn + 1); 

            for(int i=0; i<energyQuantity; i++)
            {
                int energyIndex = UnityEngine.Random.Range(0, energyGenerationRules.availableEnergies.Count());
                EnergyData energyData = energyGenerationRules.availableEnergies[energyIndex];
                totals.TryGetValue(energyData.EnergyColor, out var current);
                totals[energyData.EnergyColor] = current + 1;
            }

            List<EnergyQuantityData> results = new(totals.Count);

            foreach(var kvp in totals)
                results.Add(new EnergyQuantityData(kvp.Key, kvp.Value));

            return results;
        }
        #endregion 

        #region Routines
        private IEnumerator CreateEnergyRoutine(List<EnergyQuantityData> energyQuantityDataList)
        {
            foreach(EnergyQuantityData data in energyQuantityDataList)
            {
                for(int i=0; i < data.Quantity; i++)
                {
                    EnergyData energyData = availableEnergies.FirstOrDefault(energy => energy.EnergyColor == data.EnergyColor);
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
    public class EnergyGenerationParameters
    {
        public int turns;
        public readonly int maxEnergiesSpawn;
        public readonly int minEnergiesSpawn;
        public readonly List<EnergyData> availableEnergies;
        public EnergyGenerationParameters(int newTurns, int newMaxEnergiesSpawn, int newMinEnergiesSpawn, List<EnergyData> newAvailableEnergies)
        {
            turns = newTurns;
            maxEnergiesSpawn = newMaxEnergiesSpawn;
            minEnergiesSpawn = newMinEnergiesSpawn;
            availableEnergies = newAvailableEnergies;
        }

    }
}