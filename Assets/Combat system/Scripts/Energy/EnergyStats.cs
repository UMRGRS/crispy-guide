using System;
using NueGames.NueDeck.Scripts.Enums;

namespace NueGames.NueDeck.Scripts.Energy
{
    public class EnergyStats
    {
        public EnergyColor EnergyColor { get; private set; }
        public EnergyStrength EnergyStrength { get; private set; }
        public int BlockTurns { get; private set; }
        public Action OnInert;
        public Action OnEnergyUnblock;
        public Action OnEnergyStrengthModification;

        #region setup
        public EnergyStats(EnergyColor energyColor, EnergyStrength energyStrength, int blockTurns)
        {
            EnergyColor = energyColor;
            EnergyStrength = energyStrength;
            BlockTurns = blockTurns;
        }
        #endregion

        #region public methods
        public void ModifyStrength(EnergyModificationType modificationType)
        {
            EnergyStrength = EnergyStrengthHelper.GetNewEnergyStrengthValue(EnergyStrength, modificationType);
            if(EnergyStrength == EnergyStrength.Inert)
            {
                OnInert?.Invoke();
                return;
            }
            OnEnergyStrengthModification?.Invoke();
        }
        public void ModifyBlockTurns(int turns, BlockTurnsModificationType modificationType)
        {
            int modificationAmount = modificationType == BlockTurnsModificationType.Increase ? turns : turns * -1;
            BlockTurns += modificationAmount;
        }
        public void ReduceBlockTurns()
        {
            if(BlockTurns-- <= 0) OnEnergyUnblock?.Invoke();
        }
        #endregion
    }
}