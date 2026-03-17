using System.Text;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Utils;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "New burn damage action", menuName = "NueDeck/Collection/Actions/Status/Burn damage",order = 0)]
    public class BurnDamageAction : CardActionData
    {
        [Header("Buff energy generation settings")]
        [Range(1,10)][SerializeField] private int value;
        [Range(1,10)][SerializeField] private int turns;
        public int Value => value;
        public int Turns => turns;

        public override void Execute(CardExecutionContext context)
        {
            PayCost(context);

            context.target.CharacterStats.ApplyStatus(StatusType.BurnDamage, CalculateActionValue(context), turns:turns);

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
            var valueWord = PluralizingHelper.GetPluralizingString(actionValue, "turn", "turns");
            return BuildActionDescription($"Apply {actionValue} burn during {turns} {valueWord}");
        }
    }
}