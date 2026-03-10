using System;
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Data.Containers;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
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
        [SerializeField] private EnergyGenerationParameters energyModificationRules;
        [SerializeField] private EnergyBlockParameters energyBlockRules;
        [SerializeField] private List<CardData> currentCardsList;
        [SerializeField] private List<AllyHealthData> allyHealthDataDataList;
        [SerializeField] private bool isBossEncounter;

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
        private void InitData()
        {
            DrawCount = _gameplayData.DrawCount;
            CanUseCards = true;
            CanSelectCards = false;
            AllyList = new List<AllyBase>(_gameplayData.InitialAllyList);
            currentEncounter = new EnemyEncounter();
            remainingActiveTurns = 1;
            energyModificationRules = null;
            energyBlockRules = new EnergyBlockParameters();
            currentFloor = FloorId.firstFloor;
            CurrentCardsList = new List<CardData>();
            allyHealthDataDataList = new List<AllyHealthData>();
        }

        #region Encapsulation

        public int DrawCount{get => drawCount; set => drawCount = value;}
        public bool CanUseCards{get => canUseCards; set => canUseCards = value;}
        public bool CanSelectCards{get => canSelectCards; set => canSelectCards = value;}
        public List<AllyBase> AllyList{get => allyList; set => allyList = value;}
        public EnemyEncounter CurrentEncounter{get => currentEncounter; set => currentEncounter = value;}
        public int RemainingActiveTurns{get => remainingActiveTurns; set => remainingActiveTurns = value;}
        public EnergyGenerationParameters EnergyModificationRules{get => energyModificationRules; set => energyModificationRules = value;}
        public EnergyBlockParameters EnergyBlockRules{get => energyBlockRules; set => energyBlockRules = value;}
        public FloorId CurrentFloor{get => currentFloor; set => currentFloor = value;}
        public List<CardData> CurrentCardsList{get => currentCardsList; set => currentCardsList = value;}
        public List<AllyHealthData> AllyHealthDataList{get => allyHealthDataDataList; set => allyHealthDataDataList = value;}
        public bool IsBossEncounter { get => isBossEncounter; set => isBossEncounter = value; }

        #endregion
    }

    public class EnergyBlockParameters
    {
        private int turns = 0;
        public int Turns { get => turns; set => turns = value; }
    }
}