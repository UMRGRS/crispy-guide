using System;
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Data.Containers;
using NueGames.NueDeck.Scripts.Data.Energy;
using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Settings
{
    [Serializable]
    public class PersistentGameplayData
    {
        private readonly GameplayData _gameplayData;
        [SerializeField] private int drawCount;
        [SerializeField] private bool canUseCards;
        [SerializeField] private bool canSelectCards;
        [SerializeField] private List<AllyBase> allyList;
        [SerializeField] private FloorId currentFloor;
        [SerializeField] private EnemyEncounter currentEncounter;
        [SerializeField] private int remainingActiveTurns;
        [SerializeField] private EnergyGenerationRules energyGenerationRules;
        [SerializeField] private EnergyBlockParameters energyBlockRules;
        [SerializeField] private List<CardData> currentCardsList;
        [SerializeField] private List<AllyHealthData> allyHealthDataDataList;
        [SerializeField] private bool isBossEncounter;

        #region Encapsulation

        public int DrawCount{get => drawCount; set => drawCount = value;}
        public bool CanUseCards{get => canUseCards; set => canUseCards = value;}
        public bool CanSelectCards{get => canSelectCards; set => canSelectCards = value;}
        public List<AllyBase> AllyList{get => allyList; set => allyList = value;}
        public EnemyEncounter CurrentEncounter{get => currentEncounter; set => currentEncounter = value;}
        public int RemainingActiveTurns{
            get => remainingActiveTurns; 
            set 
            {
                if(value < 0)
                {
                    remainingActiveTurns = 0;
                }
                else
                {
                    remainingActiveTurns = value;
                }
            }
        }
        public EnergyGenerationRules EnergyGenerationRules{get => energyGenerationRules; set => energyGenerationRules = value;}
        public EnergyBlockParameters EnergyBlockRules{get => energyBlockRules; set => energyBlockRules = value;}
        public FloorId CurrentFloor{get => currentFloor; set => currentFloor = value;}
        public List<CardData> CurrentCardsList{get => currentCardsList; set => currentCardsList = value;}
        public List<AllyHealthData> AllyHealthDataList{get => allyHealthDataDataList; set => allyHealthDataDataList = value;}
        public bool IsBossEncounter { get => isBossEncounter; set => isBossEncounter = value; }

        #endregion

        public PersistentGameplayData(GameplayData gameplayData)
        {
            _gameplayData = gameplayData;

            InitData();
        }
        public void SetAllyHealthData(string id,int newCurrentHealth, int newMaxHealth)
        {
            var data = allyHealthDataDataList.Find(x => x.CharacterId == id);
            var newData = new AllyHealthData
            {
                CharacterId = id,
                CurrentHealth = newCurrentHealth,
                MaxHealth = newMaxHealth
            };
            if (data != null)
            {
                allyHealthDataDataList.Remove(data);
                allyHealthDataDataList.Add(newData);
            }
            else
            {
                allyHealthDataDataList.Add(newData);
            }
        } 
        public void InitData()
        {
            drawCount = _gameplayData.DrawCount;
            canUseCards = true;
            canSelectCards = false;
            allyList = new List<AllyBase>(_gameplayData.InitialAllyList);
            currentEncounter = new EnemyEncounter();
            remainingActiveTurns = 1;
            energyGenerationRules = new EnergyGenerationRules();
            energyBlockRules = new EnergyBlockParameters();
            currentFloor = FloorId.firstFloor;
            currentCardsList = new List<CardData>();
            allyHealthDataDataList = new List<AllyHealthData>();
        }
    }
    public class EnergyBlockParameters
    {
        private int turns = 0;
        public int Turns { get => turns; set => turns = value; }
    }
    public class EnergyGenerationRules
    {
        public int turns = 0;
        public readonly int maxEnergiesSpawn;
        public readonly int minEnergiesSpawn;
        public readonly List<EnergyData> availableEnergies;
        public EnergyGenerationRules(){}
        public EnergyGenerationRules(int newTurns, int newMaxEnergiesSpawn, int newMinEnergiesSpawn, List<EnergyData> newAvailableEnergies)
        {
            turns = newTurns;
            maxEnergiesSpawn = newMaxEnergiesSpawn;
            minEnergiesSpawn = newMinEnergiesSpawn;
            availableEnergies = newAvailableEnergies;
        }
    }
}