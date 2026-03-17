using NueGames.NueDeck.Scripts.Managers;
using NueGames.NueDeck.Scripts.Utils;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "New block energy generation action", menuName = "NueDeck/Collection/Actions/Energy/Block energy generation",order = 0)]
    public class BlockEnergyGenerationAction : CardActionData
    {
        [Header("Block energy generation settings")]
        [Range(1,10)] [SerializeField] private int turns;
        public int Turns => turns;
        public override bool CanExecute(CardExecutionContext context)
        {
            if(!context.managersContainer.EnergyPoolManager.IsEnergyOnPool(GetTotalCost())) return false;

            return true; 
        }

        public override void Execute(CardExecutionContext context)
        {
            PayCost(context);
            
            context.managersContainer.GameManager.PersistentGameplayData.EnergyBlockRules.Turns = turns;

            // Add FX effects

            // Add audio effects
        }

        public override string GetActionDescription(CardExecutionContext context)
        {
            var valueWord = PluralizingHelper.GetPluralizingString(turns, "turn", "turns");
            return BuildActionDescription($"Block start of turn energy generation for {turns} {valueWord}");
        }
    }
}