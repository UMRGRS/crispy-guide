using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "New convert energy action", menuName = "NueDeck/Collection/Actions/Energy/Convert energy",order = 0)]
    public class ConvertEnergyAction : CardActionData
    {
        [Header("Convert energy settings")]
        [SerializeField] private EnergyQuantityData from;
        [SerializeField] private EnergyQuantityData to;

        public EnergyQuantityData From => from;
        public EnergyQuantityData To => to;

        public override void Execute(CardExecutionContext context)
        {
            context.managersContainer.EnergyPoolManager.ConvertEnergy(from, to);

            // Add FX effects

            // Add audio effects
        }
        public override List<EnergyQuantityData> GetTotalCost()
        {
            return new List<EnergyQuantityData> {from, to}.Concat(GetActivationCost()).ToList();
        }
    }
}