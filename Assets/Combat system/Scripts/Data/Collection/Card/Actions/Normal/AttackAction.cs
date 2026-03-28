using System.Text;
using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "New attack action", menuName = "NueDeck/Collection/Actions/Normal/Attack",order = 0)]
    public class AttackAction : CardActionData
    {
        [Header("Attack settings")]
        [SerializeField] private int value;
        [SerializeField] private bool isSelfDamage;

        public int Value => value;
        public override void Execute(CardExecutionContext context)
        {
            PayCost(context);

            int damageToDeal = CalculateActionValue(context);

            context.target.CharacterStats.Damage(Mathf.RoundToInt(damageToDeal));
            
            if(context.registerScore)
                context.managersContainer.ScoreManager.DamageDone += damageToDeal;

            if (context.source.CharacterStats.StatusDict[StatusType.NextCardDamageBoost].IsActive && !isSelfDamage)
                context.source.CharacterStats.ClearStatus(StatusType.NextCardDamageBoost);
            
            if (context.managersContainer.FxManager != null) 
                context.managersContainer.FxManager.PlayFx(context.target.transform, FxType.Attack);
            
            if (context.managersContainer.AudioManager != null) 
                context.managersContainer.AudioManager.PlayOneShot(audioType);
        }
        public override int CalculateActionValue(CardExecutionContext context)
        {
            int upToValue = IsCostUpToValue ? upToModValue : 1;
            int buffsValue = context.source.CharacterStats.StatusDict[StatusType.PermanentDamageBoost].StatusValue +
                            context.source.CharacterStats.StatusDict[StatusType.TemporalDamageBoost].StatusValue +
                            context.source.CharacterStats.StatusDict[StatusType.NextCardDamageBoost].StatusValue;

            buffsValue = !isSelfDamage ? buffsValue : 0;

            return value * upToValue + buffsValue;
        }

        public override string GetActionDescription(CardExecutionContext context)
        {
            var suffix =  new StringBuilder("damage");

            if(isSelfDamage) suffix.Append(" to your self");

            return BuildActionDescription($"Deal {CalculateActionValue(context)} {suffix}");
        }
    }
}