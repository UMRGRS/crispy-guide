using System;
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Energy
{
    public class EnergyStats
    {
        public EnergyColor EnergyColor { get; private set; }
        public EnergyStrength EnergyStrength { get; private set; }

        #region setup
        public EnergyStats(EnergyColor energyColor, EnergyStrength energyStrength)
        {
            EnergyColor = energyColor;
            EnergyStrength = energyStrength;
        }
        #endregion

        #region public methods
        public void ModifyStrength()
        {
            throw new NotImplementedException();
        }
        #endregion

    }       
}