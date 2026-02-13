using System;
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Data.Energy;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Energy
{
    public class EnergyBase : MonoBehaviour
    {
        [Header("Energy references")]
        [SerializeField] private EnergyData activeEnergyData;

        public EnergyData ActiveEnergyData => activeEnergyData;
    }
}