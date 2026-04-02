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
        [SerializeField] private Animator energyAnimator;

        public EnergyData ActiveEnergyData => activeEnergyData;
        public EnergyStats EnergyStats { get; protected set; }
        public EnergyPoolManager EnergyPoolManager => EnergyPoolManager.Instance;
        public void BuildEnergy()
        {
            EnergyStats = new EnergyStats(activeEnergyData.EnergyColor, activeEnergyData.StartingStrength, 0);
            EnergyStats.OnInert += OnDestroy;
            EnergyStats.OnEnergyUnblock += UnblockEnergy;
            EnergyStats.OnEnergyStrengthModification += TriggerStrengthChangeAnimation;
        }

        public void BuildEnergy(EnergyStrength startingStrength)
        {
            EnergyStats = new EnergyStats(activeEnergyData.EnergyColor, startingStrength, 0);
            EnergyStats.OnInert += OnDestroy;
            EnergyStats.OnEnergyUnblock += UnblockEnergy;
            EnergyStats.OnEnergyStrengthModification += TriggerStrengthChangeAnimation;
        }
        public void BlockEnergy(int blockTurns)
        {
            EnergyStats.ModifyBlockTurns(blockTurns, BlockTurnsModificationType.Increase);
            EnergyPoolManager.OnBlockEnergy += EnergyStats.ReduceBlockTurns;
        }
        public void UnblockEnergy()
        {
            EnergyPoolManager.OnBlockEnergy -= EnergyStats.ReduceBlockTurns;
        }
        public void OnDestroy()
        {
            //Add sound if necessary
            EnergyPoolManager.RemoveEnergyFromPool(this);
            Destroy(gameObject);
        }
        public void TriggerStrengthChangeAnimation()
        {
            energyAnimator.SetTrigger(EnergyStats.EnergyStrength.ToString());
        }
    }
}