using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NueGames.NueDeck.Scripts.Enums;

namespace NueGames.NueDeck.Scripts.Card
{ 
    public static class CardActionProcessor
    {
        private static readonly Dictionary<CardActionType, ICardAction> CardActionDict =
            new();

        public static bool IsInitialized { get; private set; }

        public static void Initialize()
        {
            CardActionDict.Clear();

            var allActionCards = Assembly.GetAssembly(typeof(ICardAction)).GetTypes()
                .Where(t => typeof(ICardAction).IsAssignableFrom(t) && t.IsAbstract == false);

            foreach (var actionCard in allActionCards)
            {
                ICardAction action = Activator.CreateInstance(actionCard) as ICardAction;
                if (action != null) CardActionDict.Add(action.ActionType, action);
            }

            IsInitialized = true;
        }

        public static ICardAction GetAction(CardActionType targetAction) =>
            CardActionDict[targetAction];

    }
}