using System;
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Data.Energy;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Energy
{
    public class EnergyBase : MonoBehaviour
    {
        [Header("Energy references")]
        [SerializeField] private EnergyData activeEnergyData;

        public EnergyData ActiveEnergyData => activeEnergyData;
        public EnergyStats EnergyStats { get; protected set; }
        public EnergyPoolManager EnergyPoolManager => EnergyPoolManager.Instance;


        public void BuildEnergy()
        {
            EnergyStats = new EnergyStats(activeEnergyData.EnergyColor, activeEnergyData.StartingStrength);
            EnergyStats.OnInert += OnDestroy;
        }

        public void BuildEnergy(EnergyStrength startingStrength)
        {
            EnergyStats = new EnergyStats(activeEnergyData.EnergyColor, startingStrength);
            EnergyStats.OnInert += OnDestroy;
        }

        public void OnDestroy()
        {
            //Add sound if necessary
            EnergyPoolManager.RemoveEnergyFromPool(this);
            Destroy(gameObject);
        }
    }
}