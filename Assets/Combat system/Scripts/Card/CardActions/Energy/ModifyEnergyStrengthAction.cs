using NueGames.NueDeck.Scripts.Enums;
using System.Linq;

namespace NueGames.NueDeck.Scripts.Card.CardActions.Energy
{
    public class ModifyEnergyStrengthAction : EnergyCardActionBase
    {
        public override EnergyCardActionType ActionType => EnergyCardActionType.ModifyEnergyStrength;
        public override void DoAction(CardEnergyActionParameters actionParameters)
        {
            if(!(actionParameters.EnergyStrengthModificationList?.Any() ?? false)) return;
            
            EnergyPoolManager.ModifyEnergyStrength(actionParameters.EnergyStrengthModificationList);

            // Add FX effects

            // Add audio effects
        }
    }
}
