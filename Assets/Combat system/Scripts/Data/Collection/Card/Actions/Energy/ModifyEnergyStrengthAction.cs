using System.Collections.Generic;
using System.Linq;
using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "New modify energy strength action", menuName = "NueDeck/Collection/Actions/Energy/Modify energy strength",order = 0)]
    public class ModifyEnergyStrengthAction : CardActionData
    {
        [Header("Modify energy strength settings")]
        [SerializeField] private EnergyQuantityData from;
        [SerializeField] private EnergyModificationType modificationType;

        public EnergyQuantityData From => from;
        public EnergyModificationType ModificationType => modificationType;

        public override void Execute(CardExecutionContext context)
        {
            PayCost(context);
            
            context.managersContainer.EnergyPoolManager.ModifyEnergyStrength(from, modificationType);

            // Add FX effects

            // Add audio effects
        }
        public override List<EnergyQuantityData> GetTotalCost()
        {
            return new List<EnergyQuantityData> {from}.Concat(GetActivationCost()).ToList();
        }
    }
}