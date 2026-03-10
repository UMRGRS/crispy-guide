using System.Collections.Generic;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    public abstract class CardActionData : ScriptableObject
    {
        [Range(0f,10f)] [SerializeField] protected float actionDelay;
        [SerializeField] private List<EnergyQuantityData> costDataList;
        [SerializeField] private bool usableWithoutCost; 
        [SerializeField] private bool optional; 

        public float ActionDelay => actionDelay;
        public List<EnergyQuantityData> CostDataList => costDataList;
        public bool UsableWithoutCost => usableWithoutCost;
        public bool Optional => optional;
        
        public abstract bool CheckCost(CardExecutionContext context);
        public abstract void PayCost(CardExecutionContext context);
        public abstract void Execute(CardExecutionContext context);
        public abstract IEnumerable<EnergyQuantityData> GetTotalCost();
        public virtual IEnumerable<EnergyQuantityData> GetActivationCost()
        {
            if (!UsableWithoutCost && costDataList is not null)
                foreach (EnergyQuantityData cost in CostDataList)
                    yield return cost;
        }
    }
}