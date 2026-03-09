using NueGames.NueDeck.Scripts.Enums;
using System.Linq;

namespace NueGames.NueDeck.Scripts.Card.CardActions.Energy
{
    public class CreateEnergyAction : EnergyCardActionBase
    {
        public override EnergyCardActionType ActionType => EnergyCardActionType.CreateEnergy;
        public override void DoAction(CardEnergyActionParameters actionParameters)
        {
            if(!(actionParameters.EnergyCreationList?.Any() ?? false)) return;
            
            EnergyPoolManager.CreateEnergy(actionParameters.EnergyCreationList);

            // Add FX effects

            // Add audio effects
        }
    }
}
