using System.Collections.Generic;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "New create energy action", menuName = "NueDeck/Collection/Actions/Energy/Create energy",order = 0)]
    public class CreateEnergyAction : CardActionData
    {
        public override bool CheckCost(CardExecutionContext context)
        {
            throw new System.NotImplementedException();
        }
        public override void PayCost(CardExecutionContext context)
        {
            throw new System.NotImplementedException();
        }

        public override void Execute(CardExecutionContext context)
        {
            throw new System.NotImplementedException();
        }
        public override IEnumerable<EnergyQuantityData> GetTotalCost()
        {
            throw new System.NotImplementedException();
        }
    }
}