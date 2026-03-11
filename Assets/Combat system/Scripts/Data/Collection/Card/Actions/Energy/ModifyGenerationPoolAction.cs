using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Data.Energy;
using NueGames.NueDeck.Scripts.Data.Settings;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "New modify generation pool action", menuName = "NueDeck/Collection/Actions/Energy/Modify generation pool",order = 0)]
    public class ModifyGenerationPoolAction : CardActionData
    {
        [Header("Modify generation pool settings")]
        [Range(1,10)] [SerializeField] private int turns;
        [Range(1,10)] [SerializeField] private int maxEnergiesSpawn;
        [Range(1,10)] [SerializeField] private int minEnergiesSpawn;
        [SerializeField] private List<EnergyData> availableEnergies;
        
        public int Turns => turns;
        public int MaxEnergiesSpawn => maxEnergiesSpawn;
        public int MinEnergiesSpawn => minEnergiesSpawn;
        public List<EnergyData> AvailableEnergies => availableEnergies;
        public override void Execute(CardExecutionContext context)
        {
            context.managersContainer.GameManager.PersistentGameplayData.EnergyGenerationRules = 
                new EnergyGenerationRules(
                    turns, 
                    maxEnergiesSpawn, 
                    minEnergiesSpawn, 
                    availableEnergies);
            // Add FX effects

            // Add audio effects
        }
    }
}