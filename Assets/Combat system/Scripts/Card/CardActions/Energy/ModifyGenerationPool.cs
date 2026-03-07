using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;

namespace NueGames.NueDeck.Scripts.Card.CardActions.Energy
{
    public class ModifyGenerationPool : EnergyCardActionBase
    {
        public override EnergyCardActionType ActionType => EnergyCardActionType.ModifyGenerationPool;
        public override void DoAction(CardEnergyActionParameters actionParameters)
        {
            if(actionParameters.ModifyEnergyGenerationPool is null) return;

            var parameters = actionParameters.ModifyEnergyGenerationPool;

            GameManager.PersistentGameplayData.EnergyModificationRules = 
                new EnergyGenerationParameters(
                    parameters.Turns, 
                    parameters.MaxEnergiesSpawn, 
                    parameters.MinEnergiesSpawn, 
                    parameters.AvailableEnergies);
            // Add FX effects

            // Add audio effects
        }
    }
}