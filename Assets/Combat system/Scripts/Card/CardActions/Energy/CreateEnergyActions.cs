using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NueGames.NueDeck.Scripts.Enums;
using System.Linq;

namespace NueGames.NueDeck.Scripts.Card.CardActions.Energy
{
    public class CreateEnergyAction : CardActionBase<CardEnergyActionParameters>
    {
        public override CardActionType ActionType => CardActionType.CreateEnergy;
        public override void DoAction(CardEnergyActionParameters actionParameters)
        {
            if(!(actionParameters.EnergyCreationList?.Any() ?? false)) return;
            
            EnergyPoolManager.CreateEnergy(actionParameters.EnergyCreationList);

            // Add FX effects

            // Add audio effects
        }
    }
}
