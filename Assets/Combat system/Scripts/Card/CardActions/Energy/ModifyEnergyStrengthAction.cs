using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NueGames.NueDeck.Scripts.Enums;
using System.Linq;

namespace NueGames.NueDeck.Scripts.Card.CardActions.Energy
{
    public class ModifyEnergyStrengthAction : CardActionBase<CardEnergyActionParameters>
    {
        public override CardActionType ActionType => CardActionType.ModifyEnergyStrength;
        public override void DoAction(CardEnergyActionParameters actionParameters)
        {
            if(!(actionParameters.EnergyStrengthModificationList?.Any() ?? false)) return;
            
            EnergyPoolManager.ModifyEnergyStrength(actionParameters.EnergyStrengthModificationList);

            // Add FX effects

            // Add audio effects
        }
    }
}
