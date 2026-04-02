using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "Player Deck Data", menuName = "NueDeck/Collection/Player Deck", order = 1)]
    public class PlayerDeckData : ScriptableObject
    {
        [SerializeField] private string deckId;
        [SerializeField] private string deckName;
        [SerializeField] private FloorId floor;
        [SerializeField] private List<CardData> cardList;

        public string DeckId => deckId;
        public string DeckName => deckName;
        public FloorId Floor => floor;
        public List<CardData> CardList => cardList;

    }
}