using System.Collections.Generic;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "New create energy action", menuName = "NueDeck/Collection/Actions/Energy/Create energy",order = 0)]
    public class CreateEnergyAction : CardActionData
    {
        [Header("Create energy settings")]
        [SerializeField] private List<EnergyQuantityData> energyToCreate;

        public override void Execute(CardExecutionContext context)
        {
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
    }
}