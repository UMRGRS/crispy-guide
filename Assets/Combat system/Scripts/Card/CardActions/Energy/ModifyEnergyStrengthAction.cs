using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NueGames.NueDeck.Scripts.Enums;

namespace NueGames.NueDeck.Scripts.Card.CardActions.Energy
{
    public class ModifyEnergyStrengthAction : CardActionBase<CardEnergyActionParameters>
    {
        public override CardActionType ActionType => CardActionType.ModifyEnergyStrength;
        public override void DoAction(CardEnergyActionParameters actionParameters)
        {
            throw new System.NotImplementedException();
        }
    }
}
