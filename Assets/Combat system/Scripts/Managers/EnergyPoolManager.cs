using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Data.Containers;
using NueGames.NueDeck.Scripts.Data.Energy;
using NueGames.NueDeck.Scripts.Data.Settings;
using NueGames.NueDeck.Scripts.Energy;
using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Managers
{
    public class EnergyPoolManager : MonoBehaviour
    {
        private EnergyPoolManager(){}
        public static EnergyPoolManager Instance {get; private set;}
        public Action OnBlockEnergy;

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
        private CombatManager CombatManager => CombatManager.Instance;
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
        public int ConsumeEnergyCost(List<EnergyQuantityContainer> costList)
        {
            int consumedEnergies = 0;
            foreach(EnergyQuantityContainer cost in costList)
            {
                List<EnergyBase> targetEnergies = FindEnergyOnPool(cost);

                foreach(EnergyBase energy in targetEnergies)
                {
                    consumedEnergies++;
                    energy.OnDestroy();
                }
            }
            return consumedEnergies;
        }
        public void CreateStartOfTurnEnergy()
        {
            EnemyEncounter currentEncounterData = GameManager.PersistentGameplayData.CurrentEncounter;
            EnergyGenerationRules modifiedEnergyGenerationParameters = GameManager.PersistentGameplayData.EnergyGenerationRules;

            if(modifiedEnergyGenerationParameters.turns > 0)
            {
                modifiedEnergyGenerationParameters.turns--;

                StartCoroutine(CreateStartOfTurnEnergyRoutine(DetermineEnergiesToCreate(modifiedEnergyGenerationParameters), 0));
            }
            else
            {
                StartCoroutine(CreateStartOfTurnEnergyRoutine(DetermineEnergiesToCreate(
                    new EnergyGenerationRules(
                        0, 
                        currentEncounterData.MaxEnergySpawn, 
                        currentEncounterData.MaxEnergySpawn, 
                        currentEncounterData.AvailableEnergies)
                        ), 0));
            }
        }
        public void CreateEnergy(List<EnergyQuantityContainer> EnergyQuantityContainerList, int modifier = 0)
        {
            if(GameManager.PersistentGameplayData.EnergyBlockRules.Turns <= 0)
                StartCoroutine(CreateEnergyRoutine(EnergyQuantityContainerList, modifier));
        }
        public void DecayAllEnergy()
        {
            for (int i = CurrentEnergyInPool.Count - 1; i >= 0; i--)
            {
                EnergyBase energy = CurrentEnergyInPool[i];

                if(energy.EnergyStats.BlockTurns <= 0) 
                    energy.EnergyStats.ModifyStrength(EnergyModificationType.Weaken);
            }
        }
        public void ConvertEnergy(EnergyQuantityContainer from, EnergyQuantityContainer to)
        {
            List<EnergyBase> energyToConvert = FindEnergyOnPool(from);
            
            foreach(EnergyBase energy in energyToConvert)
            {
                EnergyStrength energyStrength = energy.EnergyStats.EnergyStrength;
                Transform spawnPos = energy.transform;
                energy.OnDestroy();
            
                EnergyData energyData = availableEnergies.FirstOrDefault(energy => energy.EnergyColor == to.Color);
                EnergyBase clone = Instantiate(energyData.EnergyPrefab, spawnPos.position, spawnPos.rotation);
                clone.transform.SetParent(spawnPos.parent, true);
                
                clone.BuildEnergy(energyStrength);
                CurrentEnergyInPool.Add(clone);
            }
        }
        public void ModifyEnergyStrength(EnergyQuantityContainer from, EnergyModificationType modificationType)
        {
            List<EnergyBase> energyToModify = FindEnergyOnPool(from);

            foreach(EnergyBase energy in energyToModify)
            {
                energy.EnergyStats.ModifyStrength(modificationType);
            }
        }
        public void BlockEnergies(EnergyColor color, int turns)
        {
             List<EnergyBase> foundEnergies = CurrentEnergyInPool.Where(energy => energy.EnergyStats.EnergyColor == color).ToList();

            foreach(EnergyBase energy in foundEnergies)
            {
                energy.BlockEnergy(turns);    
            }
        }
        public bool CanPayCosts(List<CardActionData> cardActions)
        {
            Dictionary<EnergyColor, int> simulatedPool = new();

            foreach (EnergyColor color in Enum.GetValues(typeof(EnergyColor)))
            {
                simulatedPool[color] = CurrentEnergyInPool.Count(energy => energy.EnergyStats.EnergyColor == color && energy.EnergyStats.BlockTurns <= 0);
            }

            foreach (CardActionData action in cardActions)
            {
                foreach (EnergyQuantityContainer cost in action.GetTotalCost())
                {
                    int currentAmount = simulatedPool[cost.Color];
                    int lastValue = currentAmount;
        
                    currentAmount -= cost.Quantity;
        
                    if (currentAmount < 0)
                    {
                        if (action.IsCostUpToValue && lastValue == 0)
                        {
                            currentAmount = 0;
                        }
                        else
                        {
                            return false;
                        }
                    }
        
                    simulatedPool[cost.Color] = currentAmount;
                }
            }
        
            return true;
        }

        public bool IsEnergyOnPool(List<EnergyQuantityContainer> neededEnergy)
        {
            foreach(EnergyQuantityContainer cost in neededEnergy)
            {
                var energies = FindEnergyOnPool(cost);
                if(energies.Count() < cost.Quantity) return false;
            }
            return true;
        }
        public void RemoveEnergyFromPool(EnergyBase targetEnergy)
        {
            CurrentEnergyInPool.Remove(targetEnergy);
        } 
        #endregion

        #region Private methods
        private List<EnergyQuantityContainer> DetermineEnergiesToCreate(EnergyGenerationRules energyGenerationRules)
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

            List<EnergyQuantityContainer> results = new(totals.Count);

            foreach(var kvp in totals)
                results.Add(new(kvp.Key, kvp.Value));
            
            return results;
        }
        private List<EnergyBase> FindEnergyOnPool(EnergyQuantityContainer energiesToFind)
        {
            return CurrentEnergyInPool.
                Where(energy => energy.EnergyStats.EnergyColor == energiesToFind.Color && energy.EnergyStats.BlockTurns <= 0)
                .OrderBy(energy => energy.EnergyStats.EnergyStrength)
                .Take(energiesToFind.Quantity)
                .ToList();            
        }
        #endregion 

        #region Routines
        private IEnumerator CreateStartOfTurnEnergyRoutine(List<EnergyQuantityContainer> EnergyQuantityContainerList, int modifier = 0)
        {
            yield return StartCoroutine(CreateEnergyRoutine(EnergyQuantityContainerList, modifier));
            CombatManager.EndStartOfTurn();
        }
        private IEnumerator CreateEnergyRoutine(List<EnergyQuantityContainer> EnergyQuantityContainerList, int modifier = 0)
        {
            foreach(EnergyQuantityContainer data in EnergyQuantityContainerList)
            {
                for(int i=0; i < data.Quantity + modifier; i++)
                {
                    EnergyData energyData = availableEnergies.FirstOrDefault(energy => energy.EnergyColor == data.Color);
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