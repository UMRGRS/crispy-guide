using System.Text;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Utils;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "New buff energy generation action", menuName = "NueDeck/Collection/Actions/Status/Buff energy gen",order = 0)]
    public class BuffEnergyGenerationAction : CardActionData
    {
        [Header("Buff energy generation settings")]
        [Range(1,10)][SerializeField] private int value;
        public int Value => value;

        public override bool CanExecute(CardExecutionContext context)
        {
            if(!context.source) return false;

            if(!context.managersContainer.EnergyPoolManager.IsEnergyOnPool(GetTotalCost(), isCostUpToValue)) return false;

            return true; 
        }

        public override void Execute(CardExecutionContext context)
        {            
            PayCost(context);

            context.source.CharacterStats.ApplyStatus(StatusType.BuffEnergyGeneration, CalculateActionValue(context));

            // Add FX effects

            // Add audio effects
        }
        public override int CalculateActionValue(CardExecutionContext context)
        {
            int upToValue = IsCostUpToValue ? upToModValue : 1;
            return value * upToValue;
        }

        public override string GetActionDescription(CardExecutionContext context)
        {
            var actionValue = CalculateActionValue(context);
            var valueWord = PluralizingHelper.GetPluralizingString(actionValue, "energy", "energies");
            return BuildActionDescription($"Create {actionValue} extra {valueWord} with your next card");
        }
    }
}