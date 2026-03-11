using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "New buff damage action", menuName = "NueDeck/Collection/Actions/Normal/Buff damage",order = 0)]
    public class BuffDamageAction : CardActionData
    {
        [Range(1,10)][SerializeField] private int value;
        [Range(1,10)][SerializeField] private int activeTurns;
        [SerializeField] private bool isMultiplier;
        [SerializeField] private bool isPermanent;
        [SerializeField] private bool isSingleUse;

        public int Value => value;
        public int ActiveTurns => activeTurns;
        public bool IsMultiplier => isMultiplier;
        public bool IsPermanent => isPermanent;
        public bool IsSingleUse => isSingleUse;

        public override void Execute(CardExecutionContext context)
        {
            if(!context.target || !context.source) return;

            if(isMultiplier && isPermanent)
                //context.target.CharacterStats.ApplyStatus(StatusType.PlainPermanentDamageBoost, )

            if(isMultiplier && isSingleUse)
                return;
            
            if(isMultiplier)
                return;
            
            if(isPermanent)
                return;
            
            if(isSingleUse)
                return;
            
            //context.target.CharacterStats.ApplyStatus(StatusType.PlainPermanentDamageBoost, )

            
        }
    }
}