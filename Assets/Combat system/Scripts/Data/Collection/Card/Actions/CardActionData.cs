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
        [SerializeField] protected bool optional; 
        [Header("Cost settings")]
        [SerializeField] protected List<EnergyQuantityData> costDataList;
        [SerializeField] protected bool isCostUpToValue;
        [SerializeField] protected bool usableWithoutCost; 
        
        [Header("Fx")]
        [SerializeField] protected AudioActionType audioType;

        public float ActionDelay => actionDelay;
        public List<EnergyQuantityData> CostDataList => costDataList;
        public bool IsCostUpToValue => isCostUpToValue;
        public bool UsableWithoutCost => usableWithoutCost;
        public bool Optional => optional;

        public AudioActionType AudioType => audioType;

        [Header("Support variables")]
        protected int upToModValue = 1;
        
        public abstract void Execute(CardExecutionContext context);
        public virtual int CalculateActionValue(CardExecutionContext context)
        {
            return 1;
        }
        public virtual void PayCost(CardExecutionContext context)
        {
            if(usableWithoutCost) return;
            
            upToModValue = context.managersContainer.EnergyPoolManager.ConsumeEnergyCost(costDataList);
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