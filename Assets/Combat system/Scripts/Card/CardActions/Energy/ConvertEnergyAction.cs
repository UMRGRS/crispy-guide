using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NueGames.NueDeck.Scripts.Enums;
using System.Linq;

namespace NueGames.NueDeck.Scripts.Card.CardActions.Energy
{
    public class ConvertEnergyAction : CardActionBase<CardEnergyActionParameters>
    {
        public override CardActionType ActionType => CardActionType.ConvertEnergy;
        public override void DoAction(CardEnergyActionParameters actionParameters)
        {
            if(!(actionParameters.EnergyConversionList?.Any() ?? false)) return;
            
            EnergyPoolManager.ConvertEnergy(actionParameters.EnergyConversionList);

            // Add FX effects

            // Add audio effects
        }
    }
}
