using System;
using System.Collections.Generic;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "Enemy Deck Data", menuName = "NueDeck/Collection/Enemy Deck", order = 1)]
    public class EnemyDeckData : ScriptableObject
    {
        [SerializeField] private string deckId;
        [SerializeField] private string deckName;
        [SerializeField] private List<EnemyAbilityData> cardList;
        [SerializeField] private EnemyAbilityData defaultAbility;
        
        public List<EnemyAbilityData> CardList => cardList;
        public string DeckId => deckId;
        public string DeckName => deckName;
        public EnemyAbilityData DefaultAbility => defaultAbility;
    }

    [Serializable]
    public class EnemyAbilityData
    {
        [Header("Settings")]
        [SerializeField] private CardData card;
        [SerializeField] private bool hideActionValue;
        public CardData Card => card;
        public bool HideActionValue => hideActionValue;
    }  
}