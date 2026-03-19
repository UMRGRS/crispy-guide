using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Data.Energy;
using NueGames.NueDeck.Scripts.Enums;

namespace NueGames.NueDeck.Scripts.Utils
{
    public class CostDataToEnergyContainerHelper
    {
        public static List<EnergyQuantityContainer> ToEnergyContainerHelper(ActionCostData cost)
        {
            List<EnergyQuantityContainer> conversion = new();

            if(cost.RedCost != 0)
                conversion.Add(new(EnergyColor.Red, cost.RedCost));
            
            if(cost.BlueCost != 0)
                conversion.Add(new(EnergyColor.Blue, cost.BlueCost));
            
            if(cost.GreenCost != 0)
                conversion.Add(new(EnergyColor.Green, cost.GreenCost));
            
            return conversion;

        }
    }
}