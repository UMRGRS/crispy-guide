using System;
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Data.Characters;
using NueGames.NueDeck.Scripts.Data.Energy;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.NueExtentions;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Containers
{
    [CreateAssetMenu(fileName = "Encounter Data", menuName = "NueDeck/Containers/EncounterData", order = 4)]
    public class EncounterData : ScriptableObject
    {
        [Header("Settings")]
        [SerializeField] private FloorId floor;
        [SerializeField] private string floorName;
        [SerializeField] private List<EnemyEncounter> enemyEncounterList;
        [SerializeField] private List<EnemyEncounter> bossEncounterList;

        public FloorId Floor => floor;
        public string FloorName => floorName;
        public List<EnemyEncounter> EnemyEncounterList => enemyEncounterList;
        public List<EnemyEncounter> BossEncounterList => bossEncounterList;

        public EnemyEncounter GetEnemyEncounter(bool isBoss = false)
        {
            if (isBoss) return bossEncounterList.RandomItem();
           
            return enemyEncounterList.RandomItem();
        }
    }

    [Serializable]
    public class EnemyEncounter
    {
        [Header("Possible enemies settings")]
        [SerializeField] private List<EnemyCharacterData> availableEnemies;
        [SerializeField] [Range(0, 10)] private int minEnemiesSpawn;
        [SerializeField] [Range(0, 10)] private int maxEnemiesSpawn;
        [Header("Energy pool settings")]
        [SerializeField] private List<EnergyData> availableEnergies;
        [SerializeField] [Range(0, 10)] private int minEnergySpawn;
        [SerializeField] [Range(0, 10)] private int maxEnergySpawn;
        [Range(1,10)][SerializeField] private int energyGenerationTurns;
        [Header("Background settings")]
        [SerializeField] private BackgroundTypes targetBackgroundType;
        public List<EnemyCharacterData> AvailableEnemies => availableEnemies;
        public int MinEnemiesSpawn => minEnemiesSpawn;
        public int MaxEnemiesSpawn => maxEnemiesSpawn;
        public List<EnergyData> AvailableEnergies => availableEnergies;
        public int MinEnergySpawn => minEnergySpawn;
        public int MaxEnergySpawn => maxEnergySpawn;
        public int EnergyGenerationTurns => energyGenerationTurns;
        public BackgroundTypes TargetBackgroundType => targetBackgroundType;
    }
}