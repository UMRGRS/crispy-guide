using UnityEngine;
using NueGames.NueDeck.Scripts.Enums;
using System.Text;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "New heal action", menuName = "NueDeck/Collection/Actions/Normal/Heal",order = 0)]
    public class HealAction : CardActionData
    {
        [Header("Heal settings")]
        [SerializeField] private int value;

        public int Value => value;

        public override void Execute(CardExecutionContext context)
        {
            PayCost(context);
            
            context.target.CharacterStats.Heal(Mathf.RoundToInt(CalculateActionValue(context)));

            if (context.managersContainer.FxManager != null) 
                context.managersContainer.FxManager.PlayFx(context.target.transform, FxType.Heal);
            
            if (context.managersContainer.AudioManager != null) 
                context.managersContainer.AudioManager.PlayOneShot(audioType);
        }

        public override int CalculateActionValue(CardExecutionContext context)
        {
            int upToValue = IsCostUpToValue ? upToModValue : 1;
            upToModValue = 1;
            return value * upToValue;
        }

        public override string GetActionDescription(CardExecutionContext context)
        {
            return BuildActionDescription($"Heal {CalculateActionValue(context)} HP");
        }
    }
}