using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "New buff energy generation action", menuName = "NueDeck/Collection/Actions/Normal/Buff energy gen",order = 0)]
    public class BuffEnergyGenerationAction : CardActionData
    {
        [Header("Buff energy generation settings")]
        [Range(1,10)][SerializeField] private int value;
        public int Value => value;
        public override void Execute(CardExecutionContext context)
        {
            if(!context.target || !context.source) return;

            context.target.CharacterStats.ApplyStatus(StatusType.BuffEnergyGeneration, value);
        }
    }
}