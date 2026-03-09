using System;
using System.Collections.Generic;
using UnityEngine;
using NueGames.NueDeck.Scripts.Enums;

namespace NueGames.NueDeck.Scripts.Energy
{
    public class EnergyStrengthHelper
    {
        public static EnergyStrength GetNewEnergyStrengthValue(EnergyStrength startingValue, EnergyModificationType modificationType)
        {
            int modificationAmount = modificationType == EnergyModificationType.Strengthen ? 1 : -1;
    
            int newValue = (int)startingValue + modificationAmount;
    
            if (!Enum.IsDefined(typeof(EnergyStrength), newValue))
            {
                Debug.Log("EnergyStrength is out of valid range.");
                return startingValue;
            }
    
            return (EnergyStrength)newValue;
        }
    }
}