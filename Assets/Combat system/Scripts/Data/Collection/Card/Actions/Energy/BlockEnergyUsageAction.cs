using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Utils;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "New block energy usage action", menuName = "NueDeck/Collection/Actions/Energy/Block energy usage",order = 0)]
    public class BlockEnergyUsageAction : CardActionData
    {
        [Header("Block energy usage settings")]
        [Range(1,10)] [SerializeField] private int turns;
        [SerializeField] private EnergyColor color;

        public int Turns => turns;
        public EnergyColor Color => color;
        
        public override bool CanExecute(CardExecutionContext context)
        {
            if(!context.managersContainer.EnergyPoolManager.IsEnergyOnPool(GetTotalCost(), isCostUpToValue)) return false;

            return true; 
        }
        public override void Execute(CardExecutionContext context)
        {
            PayCost(context);
            
            context.managersContainer.EnergyPoolManager.BlockEnergies(color, turns);

            // Add FX effects

            // Add audio effects
        }

        public override string GetActionDescription(CardExecutionContext context)
        {
            var valueWord = PluralizingHelper.GetPluralizingString(turns, "turn", "turns");
            return BuildActionDescription($"Block {color} energies during {turns} {valueWord}");
        }
    }
}