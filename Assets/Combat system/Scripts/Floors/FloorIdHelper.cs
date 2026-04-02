using System;
using System.Collections.Generic;
using UnityEngine;
using NueGames.NueDeck.Scripts.Enums;

namespace NueGames.NueDeck.Scripts.Floors
{
    public class FloorIdHelper
    {
        public static FloorId GetNewFloorId(FloorId startingValue, FloorDirection direction)
        {
            int modificationAmount = direction == FloorDirection.Up ? 1 : -1;
    
            int newValue = (int)startingValue + modificationAmount;
    
            if (!Enum.IsDefined(typeof(FloorId), newValue))
            {
                Debug.Log("EnergyStrength is out of valid range.");
                return startingValue;
            }
    
            return (FloorId)newValue;
        }
    }
}