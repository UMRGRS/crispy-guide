using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "New create energy action", menuName = "NueDeck/Collection/Actions/Energy/Create energy",order = 0)]
    public class CreateEnergyAction : CardActionData
    {
        [Header("Create energy settings")]
        [SerializeField] private List<EnergyQuantityData> energyToCreate;

        public override bool CanExecute(CardExecutionContext context)
        {
            if(!context.managersContainer.EnergyPoolManager.IsEnergyOnPool(GetTotalCost())) return false;

            return true; 
        }

        public override void Execute(CardExecutionContext context)
        {
            PayCost(context);
            
            if (context.source.CharacterStats.StatusDict[Enums.StatusType.BuffEnergyGeneration].IsActive)
            {
                context.managersContainer.EnergyPoolManager.CreateEnergy(energyToCreate, context.source.CharacterStats.StatusDict[Enums.StatusType.BuffEnergyGeneration].StatusValue);
                context.source.CharacterStats.ClearStatus(Enums.StatusType.BuffEnergyGeneration);
            }
            else
            {
                context.managersContainer.EnergyPoolManager.CreateEnergy(energyToCreate);
            }
            
            // Add FX effects

            // Add audio effects
        }

        public override string GetActionDescription(CardExecutionContext context)
        {
            var description = new StringBuilder("Create ");

            for(int i=0; i < energyToCreate.Count; i++)
            {
                if(i == energyToCreate.Count - 1 && i > 0)
                    description.Append(" and ");
                else if(i > 0)
                    description.Append(", ");
                
                description.Append($"{energyToCreate[i].Quantity} {energyToCreate[i].Color}");
            }

            return BuildActionDescription(description.ToString());
        }
    }
}