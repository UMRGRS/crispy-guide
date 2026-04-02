using System.Collections.Generic;
using System.Linq;
using NueGames.NueDeck.Scripts.Data.Energy;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "New convert energy action", menuName = "NueDeck/Collection/Actions/Energy/Convert energy",order = 0)]
    public class ConvertEnergyAction : CardActionData
    {
        [Header("Convert energy settings")]
        [SerializeField] private EnergyQuantityContainer from;
        [SerializeField] private EnergyQuantityContainer to;

        public EnergyQuantityContainer From => from;
        public EnergyQuantityContainer To => to;
        
        public override bool CanExecute(CardExecutionContext context)
        {
            if(!context.managersContainer.EnergyPoolManager.IsEnergyOnPool(GetTotalCost(), isCostUpToValue)) return false;

            return true; 
        }
        public override void Execute(CardExecutionContext context)
        {
            PayCost(context);
            
            context.managersContainer.EnergyPoolManager.ConvertEnergy(from, to);

            // Add FX effects

            // Add audio effects
        }

        public override List<EnergyQuantityContainer> GetTotalCost()
        {
            return new List<EnergyQuantityContainer> { from }.Concat(GetActivationCost()).ToList();
        }

        public override string GetActionDescription(CardExecutionContext context)
        {
            return BuildActionDescription($"Convert {from.Quantity} {from.Color} to {to.Quantity} {to.Color}");
        }
    }
}