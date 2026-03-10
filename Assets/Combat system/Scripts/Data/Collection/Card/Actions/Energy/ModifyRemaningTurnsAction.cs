using System.Collections.Generic;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "New modify remaining turns action", menuName = "NueDeck/Collection/Actions/Energy/Modify remaining turns",order = 0)]
    public class ModifyRemainingTurnsAction : CardActionData
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