using System;
using NueGames.NueDeck.Scripts.Enums;

namespace NueGames.NueDeck.Scripts.Energy
{
    public class EnergyStats
    {
        public EnergyColor EnergyColor { get; private set; }
        public EnergyStrength EnergyStrength { get; private set; }
        public Action OnInert;

        #region setup
        public EnergyStats(EnergyColor energyColor, EnergyStrength energyStrength)
        {
            EnergyColor = energyColor;
            EnergyStrength = energyStrength;
        }
        #endregion

        #region public methods
        public void ModifyStrength(EnergyModificationType modificationType)
        {
            EnergyStrength = EnergyStrengthHelper.GetNewEnergyStrengthValue(EnergyStrength, modificationType);
            if(EnergyStrength == EnergyStrength.Inert)
                OnInert?.Invoke();
        }
        #endregion

    }       
}