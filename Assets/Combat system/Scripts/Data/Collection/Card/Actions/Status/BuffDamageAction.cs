using System.Linq;
using System.Text;
using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "New buff damage action", menuName = "NueDeck/Collection/Actions/Status/Buff damage",order = 0)]
    public class BuffDamageAction : CardActionData
    {
        [Header("Buff damage settings")]
        [Range(1,10)][SerializeField] private int value;
        [Range(1,10)][SerializeField] private int activeTurns;
        [SerializeField] private bool isPermanent;
        [SerializeField] private bool isSingleUse;  

        public int Value => value;
        public int ActiveTurns => activeTurns;
        public bool IsPermanent => isPermanent;
        public bool IsSingleUse => isSingleUse;

        public override bool CanExecute(CardExecutionContext context)
        {
            if(!context.source) return false;

            if(!context.managersContainer.EnergyPoolManager.IsEnergyOnPool(GetTotalCost())) return false;

            return true; 
        }

        public override void Execute(CardExecutionContext context)
        {
            PayCost(context);
            
            if (isPermanent)
                context.source.CharacterStats.ApplyStatus(StatusType.PermanentDamageBoost, CalculateActionValue(context));
            else if (isSingleUse)
                context.source.CharacterStats.ApplyStatus(StatusType.NextCardDamageBoost, CalculateActionValue(context));
            else
                context.source.CharacterStats.ApplyStatus(StatusType.TemporalDamageBoost, CalculateActionValue(context), turns:activeTurns);
            
            
            // Add FX effects

            // Add audio effects
        }

        public override int CalculateActionValue(CardExecutionContext context)
        {
            int upToValue = isCostUpToValue ? upToModValue : 1;
            upToModValue = 1;
            return value * upToValue;
        }

        public override string GetActionDescription(CardExecutionContext context)
        {
            var suffix = new StringBuilder(" damage");

            if (isPermanent)
                suffix.Append(" permanently");
            else if (isSingleUse)
                suffix.Append(" to the next card");
            else
                suffix.Append($" during {activeTurns} turns");

            return BuildActionDescription($"Add + {CalculateActionValue(context)} {suffix}");
        }
    }
}