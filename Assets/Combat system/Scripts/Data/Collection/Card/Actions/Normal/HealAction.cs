using System.Collections.Generic;
using UnityEngine;
using NueGames.NueDeck.Scripts.Enums;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "New heal action", menuName = "NueDeck/Collection/Actions/Normal/Heal",order = 0)]
    public class HealAction : CardActionData
    {
        [SerializeField] private int healValue;

        public int HealValue => healValue;

        public override void Execute(CardExecutionContext context)
        {
            if(!context.target) return;
            context.target.CharacterStats.Heal(Mathf.RoundToInt(healValue));

            if (context.managersContainer.FxManager != null) 
                context.managersContainer.FxManager.PlayFx(context.target.transform, FxType.Heal);
            
            if (context.managersContainer.AudioManager != null) 
                context.managersContainer.AudioManager.PlayOneShot(audioType);
        }
    }
}