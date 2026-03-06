using NueGames.NueDeck.Scripts.Enums;
using System.Linq;

namespace NueGames.NueDeck.Scripts.Card.CardActions.Energy
{
    public class ModifyRemainingTurns : EnergyCardActionBase
    {
        public override EnergyCardActionType ActionType => EnergyCardActionType.ModifyRemainingTurns;
        public override void DoAction(CardEnergyActionParameters actionParameters)
        {
            if(actionParameters.RemainingTurnsModification is null) return;

            GameManager.ModifyRemainingTurns(actionParameters.RemainingTurnsModification.Value, actionParameters.RemainingTurnsModification.Type);
            // Add FX effects

            // Add audio effects
        }
    }
}
