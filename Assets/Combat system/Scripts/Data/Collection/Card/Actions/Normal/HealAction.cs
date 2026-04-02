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

            int healToDeal = CalculateActionValue(context);
            
            context.source.CharacterStats.Heal(Mathf.RoundToInt(healToDeal));

            if(context.registerScore)
                context.managersContainer.ScoreManager.Healing += healToDeal;

            if (context.managersContainer.FxManager != null) 
                context.managersContainer.FxManager.PlayFx(context.source.transform, FxType.Heal);
            
            if (context.managersContainer.AudioManager != null) 
                context.managersContainer.AudioManager.PlayOneShot(audioType);
        }

        public override bool CanExecute(CardExecutionContext context)
        {
            if(!context.source) return false;

            if(!context.managersContainer.EnergyPoolManager.IsEnergyOnPool(GetTotalCost())) return false;

            return true; 
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