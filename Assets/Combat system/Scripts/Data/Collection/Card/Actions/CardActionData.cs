using System;
using System.Collections.Generic;
using System.Text;
using NueGames.NueDeck.Scripts.Data.Energy;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Utils;
using Unity.VisualScripting;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    public abstract class CardActionData : ScriptableObject
    {
        [Header("Action Profile")] 
        [Range(0f,10f)] [SerializeField] protected float actionDelay;
        
        [Header("Cost settings")]
        [SerializeField] protected ActionCostData costData;
        [SerializeField] protected bool isCostUpToValue;
        [SerializeField] protected bool usableWithoutCost; 
        
        [Header("Fx")]
        [SerializeField] protected AudioActionType audioType;

        public float ActionDelay => actionDelay;
        public ActionCostData CostData => costData;
        public bool IsCostUpToValue => isCostUpToValue;
        public bool UsableWithoutCost => usableWithoutCost;
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

            upToModValue = context.managersContainer.EnergyPoolManager.ConsumeEnergyCost(GetCostAsQuantityContainer());
        }
        public virtual List<EnergyQuantityContainer> GetTotalCost()
        {
            return GetActivationCost();
        }
        public virtual List<EnergyQuantityContainer> GetActivationCost()
        {
            if(usableWithoutCost) return new();
            
            return GetCostAsQuantityContainer();
        }
        protected string BuildActionDescription(
            string baseDescription)
        {
            var description = new StringBuilder(baseDescription);
        
            if (isCostUpToValue)
                description.Append($" per energy consumed (max {costData.RedCost + costData.GreenCost + costData.BlueCost})");
        
            return description.ToString();
        }
        protected List<EnergyQuantityContainer> GetCostAsQuantityContainer()
        {
            return CostDataToEnergyContainerHelper.ToEnergyContainerHelper(costData);
        }
    }

    [Serializable]
    public class ActionCostData
    {
        [SerializeField] [Range(0, 10)] private int redCost;
        [SerializeField] [Range(0, 10)] private int blueCost;
        [SerializeField] [Range(0, 10)] private int greenCost;

        public int RedCost => redCost;
        public int BlueCost => blueCost;
        public int GreenCost => greenCost;

        public ActionCostData(int newRedCost = 0, int newBlueCost = 0, int newGreenCost = 0)
        {
            redCost = newRedCost;
            blueCost = newBlueCost;
            greenCost = newGreenCost;
        }
    }
}