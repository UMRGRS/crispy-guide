using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Data.Collection;

namespace NueGames.NueDeck.Scripts.Interfaces
{
    public interface IEnergyCost
    {
        public IEnumerable<EnergyQuantityData> GetActivationCost();
    }
}