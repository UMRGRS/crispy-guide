using System;
using System.Collections.Generic;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "New create energy action", menuName = "NueDeck/Collection/Actions/Energy/Create energy",order = 0)]
    public class CreateEnergyAction : CardActionData
    {
        [SerializeField] private List<EnergyQuantityData> energyToCreate;

        public override void Execute(CardExecutionContext context)
        {
            context.managersContainer.EnergyPoolManager.CreateEnergy(energyToCreate);
            
            // Add FX effects

            // Add audio effects
        }
    }
}