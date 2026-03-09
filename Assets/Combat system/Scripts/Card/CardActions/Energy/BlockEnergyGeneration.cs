using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;

namespace NueGames.NueDeck.Scripts.Card.CardActions.Energy
{
    public class BlockEnergyGeneration : EnergyCardActionBase
    {
        public override EnergyCardActionType ActionType => EnergyCardActionType.BlockEnergyGeneration;
        public override void DoAction(CardEnergyActionParameters actionParameters)
        {
            if(actionParameters.BlockEnergyGeneration is null) return;
            
            var parameters = actionParameters.BlockEnergyGeneration;

            GameManager.PersistentGameplayData.EnergyBlockRules = new EnergyBlockParameters(parameters.Turns);

            // Add FX effects

            // Add audio effects
        }
    }
}