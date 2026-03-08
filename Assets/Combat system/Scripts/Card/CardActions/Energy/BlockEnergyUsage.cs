using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;

namespace NueGames.NueDeck.Scripts.Card.CardActions.Energy
{
    public class BlockEnergyUsage : EnergyCardActionBase
    {
        public override EnergyCardActionType ActionType => EnergyCardActionType.BlockEnergyUsage;
        public override void DoAction(CardEnergyActionParameters actionParameters)
        {
            if(actionParameters.BlockEnergyUsage is null) return;
            
            var parameters = actionParameters.BlockEnergyUsage;

            EnergyPoolManager.BlockEnergies(parameters.Color, parameters.Turns);

            // Add FX effects

            // Add audio effects
        }
    }
}