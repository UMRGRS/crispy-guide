using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    public abstract class CardActionData : ScriptableObject
    {
        [Header("Action Profile")] 
        [Range(0f,10f)] [SerializeField] protected float actionDelay;
        [Header("Action Settings")]
        [SerializeField] protected List<EnergyQuantityData> costDataList;
        [SerializeField] protected bool usableWithoutCost; 
        [SerializeField] protected bool optional; 
        [Header("Fx")]
        [SerializeField] protected AudioActionType audioType;

        public float ActionDelay => actionDelay;
        public List<EnergyQuantityData> CostDataList => costDataList;
        public bool UsableWithoutCost => usableWithoutCost;
        public bool Optional => optional;

        public AudioActionType AudioType => audioType;
        
        public abstract void Execute(CardExecutionContext context);
        public virtual void PayCost(CardExecutionContext context)
        {
            context.managersContainer.EnergyPoolManager.ConsumeEnergyCost(costDataList);
        }
        public virtual List<EnergyQuantityData> GetTotalCost()
        {
            return GetActivationCost();
        }
        public virtual List<EnergyQuantityData> GetActivationCost()
        {
            return costDataList;
        }
    }
}