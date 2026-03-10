using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "New block energy generation action", menuName = "NueDeck/Collection/Actions/Energy/Block energy generation",order = 0)]
    public class BlockEnergyGenerationAction : CardActionData
    {
        [Range(1,10)] [SerializeField] private int turns;
        public int Turns => turns;

        public override void Execute(CardExecutionContext context)
        {
            context.managersContainer.GameManager.PersistentGameplayData.EnergyBlockRules.Turns = turns;

            // Add FX effects

            // Add audio effects
        }
    }
}