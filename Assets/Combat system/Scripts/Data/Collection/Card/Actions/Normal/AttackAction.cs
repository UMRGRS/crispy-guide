using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "New attack action", menuName = "NueDeck/Collection/Actions/Normal/Attack",order = 0)]
    public class AttackAction : CardActionData
    {
        [Header("Attack settings")]
        [SerializeField] private int value;

        public int Value => value;
        public override void Execute(CardExecutionContext context)
        {
            PayCost(context);

            if (context.source.CharacterStats.StatusDict[StatusType.NextCardDamageBoost].IsActive)
                context.source.CharacterStats.ClearStatus(StatusType.NextCardDamageBoost);
            
            context.target.CharacterStats.Damage(Mathf.RoundToInt(CalculateActionValue(context)));
            
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

            return value * upToValue + buffsValue;
        }
    }
}