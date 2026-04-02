using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Card;
using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Data.Collection;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Settings
{
    [CreateAssetMenu(fileName = "Gameplay Data", menuName = "NueDeck/Settings/GameplayData", order = 0)]
    public class GameplayData : ScriptableObject
    {
        [Header("Gameplay Settings")] 
        [SerializeField] private int drawCount = 4;
        [SerializeField] private List<AllyBase> initialAllyList;
        
        [Header("Decks")] 
        [SerializeField] private PlayerDeckData initialDeck;
        [SerializeField] [Range(1,20)] private int maxCardOnHand;
        
        [Header("Card Settings")] 
        [SerializeField] private List<CardData> allCardsList;
        [SerializeField] private CardBase cardPrefab;

        [Header("Customization Settings")] 
        [SerializeField] private string defaultName = "Nue";
        
        #region Encapsulation
        public int DrawCount => drawCount;
        public List<AllyBase> InitialAllyList => initialAllyList;
        public PlayerDeckData InitialDeck => initialDeck;
        public int MaxCardOnHand => maxCardOnHand;
        public List<CardData> AllCardsList => allCardsList;
        public CardBase CardPrefab => cardPrefab;
        public string DefaultName => defaultName;
        #endregion
    }
}