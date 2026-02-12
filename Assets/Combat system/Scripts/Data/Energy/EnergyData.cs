using System;
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Energy;
using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Energy
{
    [CreateAssetMenu(fileName = "Energy type", menuName ="NueDeck/Energy", order = 0)]
    public class EnergyData : ScriptableObject
    {
        [Header("Base")]
        [SerializeField] private EnergyBase energyPrefab;
        [SerializeField] private EnergyColor energyType;
        [SerializeField] private EnergyStrength startingStrength;

        public EnergyBase EnergyPrefab => energyPrefab;
        public EnergyColor EnergyType => energyType;
        public EnergyStrength StartingStrength => startingStrength;
    } 
}