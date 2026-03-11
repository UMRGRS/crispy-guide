using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "New buff damage action", menuName = "NueDeck/Collection/Actions/Normal/Buff damage",order = 0)]
    public class BuffDamageAction : CardActionData
    {
        [Header("Buff damage action settings")]
        [Range(1,10)][SerializeField] private int value;
        [Range(1,10)][SerializeField] private int activeTurns;
        [SerializeField] private bool isPermanent;
        [SerializeField] private bool isSingleUse;

        public int Value => value;
        public int ActiveTurns => activeTurns;
        public bool IsPermanent => isPermanent;
        public bool IsSingleUse => isSingleUse;

        public override void Execute(CardExecutionContext context)
        {
            if(!context.target || !context.source) return;

            if (isPermanent)
            {
                context.target.CharacterStats.ApplyStatus(StatusType.PermanentDamageBoost, value);
                return;
            }

            if (isSingleUse)
            {
                context.target.CharacterStats.ApplyStatus(StatusType.NextCardDamageBoost, value);
                return;
            }
            context.target.CharacterStats.ApplyStatus(StatusType.TemporalDamageBoost, value, turns:activeTurns); 
        }
    }
}