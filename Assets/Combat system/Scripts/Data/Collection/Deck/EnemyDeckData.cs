using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using NueGames.NueDeck.Scripts.Data.Containers;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "Enemy Deck Data", menuName = "NueDeck/Collection/Enemy Deck", order = 1)]
    public class EnemyDeckData : ScriptableObject
    {
        [SerializeField] private string deckId;
        [SerializeField] private string deckName;
        [SerializeField] private List<EnemyAbilityData> cardList;
        
        public List<EnemyAbilityData> CardList => cardList;
        public string DeckId => deckId;
        public string DeckName => deckName;
    }

    [Serializable]
    public class EnemyAbilityData
    {
        [Header("Settings")]
        [SerializeField] private CardData card;
        //Possible deletion
        [SerializeField] private EnemyIntentionData intention;
        [SerializeField] private bool hideActionValue;
        private bool wasUsedLastTurn;
        
        public CardData Card => card;
        public EnemyIntentionData Intention => intention;
        public bool HideActionValue => hideActionValue;
        public bool WasUsedLastTurn => wasUsedLastTurn;
        public void SetAsUsed()
        {
            wasUsedLastTurn = true;
        }
        public void SetAsUnused()
        {
            wasUsedLastTurn = false;
        }
    }  
}