using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "New modify remaining turns action", menuName = "NueDeck/Collection/Actions/Energy/Modify remaining turns",order = 0)]
    public class ModifyRemainingTurnsAction : CardActionData
    {
        [Header("BModify remaining turns settings")]
        [SerializeField] private RemainingTurnsModificationType type;
        [Range(1,10)] [SerializeField] private int value;

        public RemainingTurnsModificationType Type => type;
        public int Value => value;
        public override void Execute(CardExecutionContext context)
        {
            PayCost(context);
            
            context.managersContainer.GameManager.ModifyRemainingTurns(value, type);
            // Add FX effects

            // Add audio effects
        }
    }
}