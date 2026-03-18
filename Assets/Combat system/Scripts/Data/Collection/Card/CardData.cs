using System.Collections.Generic;
using System.Linq;
using System.Text;
using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "New card data",menuName = "NueDeck/Collection/Card",order = 0)]
    public class CardData : ScriptableObject
    {
        [Header("Card Profile")] 
        [SerializeField] private string id;
        [SerializeField] private string cardName;
        [SerializeField] private Sprite cardSprite;
        [SerializeField] private RarityType rarity;
        
        [Header("Action Settings")]
        [SerializeField] private bool usableWithoutTarget;
        [SerializeField] private ActionTargetType actionTargetType;
        [SerializeField] private List<CardActionData> cardActionDataList;
        
        [Header("Description")]
        [SerializeField] private List<SpecialKeywords> specialKeywordsList;

        #region public getters
        public string Id => id;
        public bool UsableWithoutTarget => usableWithoutTarget;
        public ActionTargetType ActionTargetType => actionTargetType;
        public string CardName => cardName;
        public Sprite CardSprite => cardSprite;
        public List<CardActionData> CardActionDataList => cardActionDataList;
        public List<SpecialKeywords> KeywordsList => specialKeywordsList;
        public string MyDescription { get; set; }
        public RarityType Rarity => rarity;
        #endregion
        
        #region public methods
        
        public void UpdateDescription(CardExecutionContext context)
        {
            var str = new StringBuilder();

            foreach (var action in cardActionDataList)
            {
                str.Append($"{action.GetActionDescription(context)} \n");
            }
            
            MyDescription = str.ToString();
        }

        public List<EnergyQuantityData> GatherCardCosts()
        {
            Dictionary<EnergyColor, int> totals = new();

            foreach(CardActionData action in cardActionDataList)
            {
                foreach(EnergyQuantityData cost in action.CostDataList)
                {
                    totals.TryGetValue(cost.EnergyColor, out var current);
                    totals[cost.EnergyColor] = current + cost.Quantity;
                }
                
            }

            List<EnergyQuantityData> results = new(totals.Count);

            foreach(var kvp in totals)
                results.Add(new EnergyQuantityData(kvp.Key, kvp.Value));

            return results;
        }

        public int GetCostNumber()
        {
            return GatherCardCosts().Sum(x=> x.Quantity);
        }
        #endregion
    }
}