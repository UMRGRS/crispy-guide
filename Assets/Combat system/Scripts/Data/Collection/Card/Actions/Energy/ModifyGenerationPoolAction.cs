using System.Collections.Generic;
using System.Text;
using NueGames.NueDeck.Scripts.Data.Energy;
using NueGames.NueDeck.Scripts.Data.Settings;
using NueGames.NueDeck.Scripts.Utils;
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

        public override bool CanExecute(CardExecutionContext context)
        {
            if(!context.managersContainer.EnergyPoolManager.IsEnergyOnPool(GetTotalCost())) return false;

            return true; 
        }
        public override void Execute(CardExecutionContext context)
        {
            PayCost(context);
            
            context.managersContainer.GameManager.PersistentGameplayData.EnergyGenerationRules = 
                new EnergyGenerationRules(
                    turns, 
                    maxEnergiesSpawn, 
                    minEnergiesSpawn, 
                    availableEnergies);
            // Add FX effects

            // Add audio effects
        }

        public override string GetActionDescription(CardExecutionContext context)
        {

            var colors = new StringBuilder();

            for(int i=0; i < availableEnergies.Count; i++)
            {
                if(i == availableEnergies.Count - 1 && i > 0)
                    colors.Append(" or ");
                else if(i > 0)
                    colors.Append(", ");
                
                colors.Append($"{availableEnergies[i].EnergyColor}");
            }
            
            var valueWord = PluralizingHelper.GetPluralizingString(turns, "turn", "turns");
            return BuildActionDescription($"The next {(turns == 1 ? "" : turns)} {valueWord} all the energies will be {colors}");
        }
    }
}