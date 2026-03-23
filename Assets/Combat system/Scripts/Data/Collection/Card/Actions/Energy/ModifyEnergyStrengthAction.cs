using System.Collections.Generic;
using System.Linq;
using NueGames.NueDeck.Scripts.Data.Energy;
using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "New modify energy strength action", menuName = "NueDeck/Collection/Actions/Energy/Modify energy strength",order = 0)]
    public class ModifyEnergyStrengthAction : CardActionData
    {
        [Header("Modify energy strength settings")]
        [SerializeField] private EnergyQuantityContainer from;
        [SerializeField] private EnergyModificationType modificationType;

        public EnergyQuantityContainer From => from;
        public EnergyModificationType ModificationType => modificationType;
        
        public override bool CanExecute(CardExecutionContext context)
        {
            if(!context.managersContainer.EnergyPoolManager.IsEnergyOnPool(GetTotalCost())) return false;

            return true; 
        }
        public override void Execute(CardExecutionContext context)
        {
            PayCost(context);
            
            context.managersContainer.EnergyPoolManager.ModifyEnergyStrength(from, modificationType);

            // Add FX effects

            // Add audio effects
        }
        public override List<EnergyQuantityContainer> GetTotalCost()
        {
            return new List<EnergyQuantityContainer> {from}.Concat(GetActivationCost()).ToList();
        }
        public override string GetActionDescription(CardExecutionContext context)
        {
            return BuildActionDescription($"{modificationType} {from.Quantity} {from.Color}");
        }
    }
}