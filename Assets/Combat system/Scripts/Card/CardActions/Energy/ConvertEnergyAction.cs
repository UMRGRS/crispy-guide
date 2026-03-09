using NueGames.NueDeck.Scripts.Enums;
using System.Linq;

namespace NueGames.NueDeck.Scripts.Card.CardActions.Energy
{
    public class ConvertEnergyAction : EnergyCardActionBase
    {
        public override EnergyCardActionType ActionType => EnergyCardActionType.ConvertEnergy;
        public override void DoAction(CardEnergyActionParameters actionParameters)
        {
            if(!(actionParameters.EnergyConversionList?.Any() ?? false)) return;
            
            EnergyPoolManager.ConvertEnergy(actionParameters.EnergyConversionList);

            // Add FX effects

            // Add audio effects
        }
    }
}
