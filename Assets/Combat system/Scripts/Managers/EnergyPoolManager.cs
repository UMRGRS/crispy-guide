using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NueGames.NueDeck.Scripts.Data.Collection;
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

        #region Cache
        public List<EnergyBase> CurrentEnergyInPool {get; private set;} = new List<EnergyBase>();
        public List<Transform> EnergyPostList => energyPosList;
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
            List<EnergyQuantityData> energyCreationData = new();
            foreach(EnergyData data in availableEnergies)
            {
                int energyQuantity = UnityEngine.Random.Range(1,5); 
                energyCreationData.Add(new EnergyQuantityData(data.EnergyType, energyQuantity));
            }
            CreateEnergy(energyCreationData);
        }
        public void CreateEnergy(List<EnergyQuantityData> energyQuantityDataList)
        {
            foreach(EnergyQuantityData data in energyQuantityDataList)
            {
                for(int i=0; i < data.Quantity; i++)
                {
                    EnergyData energyData = availableEnergies.FirstOrDefault(energy => energy.EnergyType == data.EnergyColor);
                    int spawnPosition = UnityEngine.Random.Range(0, energyPosList.Count-1);
                    var clone = Instantiate(energyData.EnergyPrefab, energyPosList[spawnPosition]);
                    clone.BuildEnergy();
                    CurrentEnergyInPool.Add(clone);
                }
            }
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
        public void RemoveEnergyFromPool(EnergyBase targetEnergy)
        {
            CurrentEnergyInPool.Remove(targetEnergy);
            //Insert energy deplete win condition
        } 

        //return a list of the specified energies if they exist on the pool
        //ej. if a card lists (red, blue) returns (red, blue) otherwise an empty array
        public bool IsEnergyOnPool(List<EnergyQuantityData> energiesToFind)
        {
            List<EnergyBase> foundEnergies = FindEnergyOnPool(energiesToFind);
            return foundEnergies?.Any() ?? false;
        }

        public void ConvertEnergy(List<EnergyConversion> energyToConvert)
        {
            //Destroy the previous color by setting strength to inert, create new one with second buildEnergy and add it to the list
            throw new NotImplementedException();
        }

        public void ModifyEnergyStrength(List<EnergyStrengthModification> energyToModifyStrength)
        {
            //Modify the strength of the passed energies
            throw new NotImplementedException();
        }
        #endregion

        #region Private methods
        private List<EnergyBase> FindEnergyOnPool(List<EnergyQuantityData> energiesToFind)
        {
            List<EnergyBase> targetEnergies = new List<EnergyBase>();
            foreach(EnergyQuantityData energyData in energiesToFind)
            {
                CurrentEnergyInPool.Where(energy => energy.EnergyStats.EnergyColor == energyData.EnergyColor);

            }

            throw new NotImplementedException();
        }
        #endregion
    }
}

