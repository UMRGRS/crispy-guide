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
            {
                context.target.CharacterStats.ApplyStatus(StatusType.PermanentDamageBoost, CalculateActionValue(context));
            }
            else if (isSingleUse)
            {
                context.target.CharacterStats.ApplyStatus(StatusType.NextCardDamageBoost, CalculateActionValue(context));
            }
            else
            {
                context.target.CharacterStats.ApplyStatus(StatusType.TemporalDamageBoost, CalculateActionValue(context), turns:activeTurns);
            }
            
            // Add FX effects

            // Add audio effects
        }

        public override int CalculateActionValue(CardExecutionContext context)
        {
            int upToValue = IsCostUpToValue ? upToModValue : 1;
            upToModValue = 1;
            return value * upToValue;
        }
    }
}