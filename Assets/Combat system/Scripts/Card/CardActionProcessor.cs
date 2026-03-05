using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NueGames.NueDeck.Scripts.Enums;

namespace NueGames.NueDeck.Scripts.Card
{ 
    public static class CardActionProcessor
    {
        private static readonly Dictionary<CardActionType, CardActionBase> CardActionDict = new();
        private static readonly Dictionary<EnergyCardActionType, EnergyCardActionBase> EnergyCardActionDict = new();

        public static bool IsInitialized { get; private set; }

        public static void Initialize()
        {
            CardActionDict.Clear();
            EnergyCardActionDict.Clear();

            var allActionCards = Assembly.GetAssembly(typeof(CardActionBase)).GetTypes()
                .Where(t => typeof(CardActionBase).IsAssignableFrom(t) && t.IsAbstract == false);

            foreach (var actionCard in allActionCards)
            {
                if (Activator.CreateInstance(actionCard) is CardActionBase action) CardActionDict.Add(action.ActionType, action);
            }
            
            var allEnergyActionCards = Assembly.GetAssembly(typeof(EnergyCardActionBase)).GetTypes()
                .Where(t => typeof(EnergyCardActionBase).IsAssignableFrom(t) && t.IsAbstract == false);

            foreach (var actionCard in allEnergyActionCards)
            {
                if (Activator.CreateInstance(actionCard) is EnergyCardActionBase action) EnergyCardActionDict.Add(action.ActionType, action);
            }

            IsInitialized = true;
        }
        public static CardActionBase GetAction(CardActionType targetAction) => CardActionDict[targetAction];
        public static EnergyCardActionBase GetEnergyAction(EnergyCardActionType targetAction) => EnergyCardActionDict[targetAction];

    }
}