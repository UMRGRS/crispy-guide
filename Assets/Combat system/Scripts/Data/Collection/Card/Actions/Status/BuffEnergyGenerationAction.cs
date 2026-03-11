using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "New buff energy generation action", menuName = "NueDeck/Collection/Actions/Status/Buff energy gen",order = 0)]
    public class BuffEnergyGenerationAction : CardActionData
    {
        [Header("Buff energy generation settings")]
        [Range(1,10)][SerializeField] private int value;
        public int Value => value;
        public override void Execute(CardExecutionContext context)
        {
            if(!context.source) return;

            context.source.CharacterStats.ApplyStatus(StatusType.BuffEnergyGeneration, value);

            // Add FX effects

            // Add audio effects
        }
    }
}