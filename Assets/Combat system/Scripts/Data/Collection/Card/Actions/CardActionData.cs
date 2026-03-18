using System.Collections.Generic;
using System.Text;
using NueGames.NueDeck.Scripts.Characters;
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
        public abstract string GetActionDescription(CardExecutionContext context);
        public virtual bool CanExecute(CardExecutionContext context)
        {
            if(!context.target || !context.source) return false;

            if(!context.managersContainer.EnergyPoolManager.IsEnergyOnPool(GetTotalCost())) return false;

            return true; 
        }
        public virtual int CalculateActionValue(CardExecutionContext context)
        {
            return 0;
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
            if(usableWithoutCost) return new();
            
            return costDataList;
        }
        protected string BuildActionDescription(
            string baseDescription)
        {
            var description = new StringBuilder(baseDescription);
        
            if (isCostUpToValue)
                description.Append($" per energy consumed (max {costDataList[0].Quantity})");
        
            if (optional)
                description.Append(" (opt)");
        
            return description.ToString();
        }
    }
}