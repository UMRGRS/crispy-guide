using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "New attack action", menuName = "NueDeck/Collection/Actions/Normal/Attack",order = 0)]
    public class AttackAction : CardActionData
    {
        [Header("Attack settings")]
        [SerializeField] private int attackValue;

        public int AttackValue => attackValue;
        public override void Execute(CardExecutionContext context)
        {
            if(!context.target || !context.source) return;

            int totalDamage = attackValue + 
                            context.source.CharacterStats.StatusDict[StatusType.PermanentDamageBoost].StatusValue +
                            context.source.CharacterStats.StatusDict[StatusType.TemporalDamageBoost].StatusValue +
                            context.source.CharacterStats.StatusDict[StatusType.NextCardDamageBoost].StatusValue;

            if (context.source.CharacterStats.StatusDict[StatusType.NextCardDamageBoost].IsActive)
                context.source.CharacterStats.ClearStatus(StatusType.NextCardDamageBoost);
            
            context.target.CharacterStats.Damage(Mathf.RoundToInt(totalDamage));
            
            if (context.managersContainer.FxManager != null) 
                context.managersContainer.FxManager.PlayFx(context.target.transform, FxType.Attack);
            
            if (context.managersContainer.AudioManager != null) 
                context.managersContainer.AudioManager.PlayOneShot(audioType);
        }
    }
}