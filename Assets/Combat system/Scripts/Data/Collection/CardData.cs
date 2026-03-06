using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Interfaces;
using NueGames.NueDeck.Scripts.Managers;
using NueGames.NueDeck.Scripts.NueExtentions;
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
        [SerializeField] private List<CardActionData> cardActionDataList;

        [Header("Energy Actions Settings")]
        [SerializeField] private List<EnergyCardActionData> cardEnergyActionDataList;
        
        [Header("Description")]
        [SerializeField] private List<CardDescriptionData> cardDescriptionDataList;
        [SerializeField] private List<SpecialKeywords> specialKeywordsList;
        
        [Header("Fx")]
        [SerializeField] private AudioActionType audioType;

        #region public getters
        public string Id => id;
        public bool UsableWithoutTarget => usableWithoutTarget;
        public string CardName => cardName;
        public Sprite CardSprite => cardSprite;
        public List<CardActionData> CardActionDataList => cardActionDataList;
        public List<EnergyCardActionData> CardEnergyActionDataList => cardEnergyActionDataList;
        public List<CardDescriptionData> CardDescriptionDataList => cardDescriptionDataList;
        public List<SpecialKeywords> KeywordsList => specialKeywordsList;
        public AudioActionType AudioType => audioType;
        public string MyDescription { get; set; }
        public RarityType Rarity => rarity;
        #endregion
        
        #region public methods
        public void UpdateDescription()
        {
            var str = new StringBuilder();

            foreach (var descriptionData in cardDescriptionDataList)
            {
                str.Append(descriptionData.UseModifier
                    ? descriptionData.GetModifiedValue(this)
                    : descriptionData.GetDescription());
            }
            
            MyDescription = str.ToString();
        }

        public List<EnergyQuantityData> GatherTotalEnergyCosts()
        {
            Dictionary<EnergyColor, int> totals = new();

            void AddCost(EnergyQuantityData cost)
            {
                if (cost == null) return;
        
                totals.TryGetValue(cost.EnergyColor, out var current);
                totals[cost.EnergyColor] = current + cost.Quantity;
            }

            if (cardEnergyActionDataList != null)
                foreach (EnergyCardActionData action in CardEnergyActionDataList)
                    foreach (EnergyQuantityData cost in action.GetEnergyCosts())
                        AddCost(cost);

            List<EnergyQuantityData> results = new(totals.Count);

            List<EnergyQuantityData> totalCost = GatherActivationCost();

            foreach(var kvp in totals)
                results.Add(new EnergyQuantityData(kvp.Key, kvp.Value));

            totalCost.AddRange(results);

            return totalCost;
        }

        public List<EnergyQuantityData> GatherActivationCost()
        {
            Dictionary<EnergyColor, int> totals = new();

            void AddCost(EnergyQuantityData cost)
            {
                if (cost == null) return;
        
                totals.TryGetValue(cost.EnergyColor, out var current);
                totals[cost.EnergyColor] = current + cost.Quantity;
            }

            if (CardActionDataList != null)
                foreach (CardActionData action in CardActionDataList)
                    foreach (EnergyQuantityData cost in action.GetActivationCost())
                        AddCost(cost);

            if (cardEnergyActionDataList != null)
                foreach (EnergyCardActionData action in CardEnergyActionDataList)
                    foreach (EnergyQuantityData cost in action.GetActivationCost())
                        AddCost(cost);

            List<EnergyQuantityData> results = new(totals.Count);

            foreach(var kvp in totals)
                results.Add(new EnergyQuantityData(kvp.Key, kvp.Value));

            return results;
        }
        #endregion


        #region Editor Methods
        #if UNITY_EDITOR
        public void EditCardName(string newName) => cardName = newName;
        public void EditId(string newId) => id = newId;
        public void EditRarity(RarityType targetRarity) => rarity = targetRarity;
        public void EditCardSprite(Sprite newSprite) => cardSprite = newSprite;
        public void EditUsableWithoutTarget(bool newStatus) => usableWithoutTarget = newStatus;
        public void EditCardActionDataList(List<CardActionData> newCardActionDataList) =>
            cardActionDataList = newCardActionDataList;
        public void EditCardEnergyActionDataList(List<EnergyCardActionData> newCardEnergyActionDataList) =>
            cardEnergyActionDataList = newCardEnergyActionDataList;
        public void EditCardDescriptionDataList(List<CardDescriptionData> newCardDescriptionDataList) =>
            cardDescriptionDataList = newCardDescriptionDataList;
        public void EditSpecialKeywordsList(List<SpecialKeywords> newSpecialKeywordsList) =>
            specialKeywordsList = newSpecialKeywordsList;
        public void EditAudioType(AudioActionType newAudioActionType) => audioType = newAudioActionType;
        #endif

        #endregion

    }

    [Serializable]
    public class EnergyQuantityData
    {
        [SerializeField] private EnergyColor energyColor;
        [Range(1, 10)] [SerializeField] private int quantity;
    
        public EnergyColor EnergyColor => energyColor;
        public int Quantity => quantity;
        
        public EnergyQuantityData(){}     
        public EnergyQuantityData(EnergyColor newColor, int newQuantity)
        {
            energyColor = newColor;
            quantity = newQuantity;
        }
           
        #region Editor

        #if UNITY_EDITOR
                public void EditCostType(EnergyColor newColor) =>  energyColor = newColor;
                public void EditQuantityCost(int newQuantity) => quantity = newQuantity;

        #endif
        #endregion
    }

    [Serializable]
    public class CardActionData : IEnergyCost
    {
        [SerializeField] private CardActionType cardActionType;
        [SerializeField] private ActionTargetType actionTargetType;
        [SerializeField] private float actionValue;
        [SerializeField] private List<EnergyQuantityData> costDataList;
        [SerializeField] private bool usableWithoutCost;
        [SerializeField] private bool optional;
        [Range(0.1f, 10)][SerializeField] private float actionDelay;
        
        public CardActionType CardActionType => cardActionType;
        public ActionTargetType ActionTargetType => actionTargetType;
        public float ActionValue => actionValue;
        public List<EnergyQuantityData> CostDataList => costDataList;
        public bool UsableWithoutCost => usableWithoutCost;
        public bool Optional => optional;
        public float ActionDelay => actionDelay;

        #region public methods
        public IEnumerable<EnergyQuantityData> GetActivationCost()
        {
            if (costDataList != null && !UsableWithoutCost)
                foreach (EnergyQuantityData cost in CostDataList)
                    yield return cost;
        }
        #endregion

        #region Editor

        #if UNITY_EDITOR
        public void EditActionType(CardActionType newType) =>  cardActionType = newType;
        public void EditActionTarget(ActionTargetType newTargetType) => actionTargetType = newTargetType;
        public void EditActionValue(float newValue) => actionValue = newValue;
        public void EditCostDataList(List<EnergyQuantityData> newCostDataList) => costDataList = newCostDataList;
        public void EditUsableWithoutCost(bool newUsableWithoutCost) => usableWithoutCost = newUsableWithoutCost;
        public void EditOptional(bool newOptional) => optional = newOptional;
        public void EditActionDelay(float newValue) => actionDelay = newValue;
        #endif
        #endregion
    }

    [Serializable]
    public class EnergyCardActionData: IEnergyCost
    {
        [SerializeField] private EnergyCardActionType cardActionType;
        [SerializeField] private List<EnergyQuantityData> energyToCreate;
        [SerializeField] private List<EnergyConversion> energyToConvert;
        [SerializeField] private List<EnergyStrengthModification> energyToModifyStrength;
        [SerializeField] private RemainingTurnsModification turnsModification;
        [SerializeField] private List<EnergyQuantityData> costDataList;
        [SerializeField] private bool usableWithoutCost;
        [SerializeField] private bool optional;
        [Range(0.1f, 10f)][SerializeField] private float actionDelay;

        public EnergyCardActionType CardActionType => cardActionType;
        public List<EnergyQuantityData> EnergyToCreate => energyToCreate;
        public List<EnergyConversion> EnergyToConvert => energyToConvert;
        public List<EnergyStrengthModification> EnergyToModifyStrength => energyToModifyStrength;
        public RemainingTurnsModification TurnsModification => turnsModification;
        public List<EnergyQuantityData> CostDataList => costDataList;
        public bool UsableWithoutCost => usableWithoutCost;
        public bool Optional => optional;
        public float ActionDelay => actionDelay;

        #region public methods
        public IEnumerable<EnergyQuantityData> GetActivationCost()
        {
            if (costDataList != null && !UsableWithoutCost)
                foreach (EnergyQuantityData cost in CostDataList)
                    yield return cost;
        }
        public IEnumerable<EnergyQuantityData> GetEnergyCosts()
        {

            if (energyToConvert != null)
                foreach (EnergyConversion conversion in energyToConvert)
                    yield return conversion.From;

            if (energyToModifyStrength != null)
                foreach (EnergyStrengthModification modification in energyToModifyStrength)
                    yield return modification.From;
        }
        #endregion

        #region Editor
        #if UNITY_EDITOR
        public void EditActionType(EnergyCardActionType newCardActionType) => cardActionType = newCardActionType;
        public void EditEnergyToCreate(List<EnergyQuantityData> newEnergyToCreate) => energyToCreate = newEnergyToCreate;
        public void EditEnergyToConvert(List<EnergyConversion>  newEnergyToConvert) => energyToConvert = newEnergyToConvert;
        public void EditEnergyToModifyStrength(List<EnergyStrengthModification> newEnergyToModifyStrength) => energyToModifyStrength = newEnergyToModifyStrength;
        public void EditCostDataList(List<EnergyQuantityData> newCostDataList) => costDataList = newCostDataList;
        public void EditTurnsModification(RemainingTurnsModification newTurnsModification) => turnsModification = newTurnsModification;
        public void EditUsableWithoutCost(bool newStatus) => usableWithoutCost = newStatus;
        public void EditOptional(bool newOptional) => optional = newOptional;
        public void EditActionDelay(float newValue) => actionDelay = newValue;
        #endif
        #endregion
    }

    [Serializable]
    public class EnergyConversion
    {
        [SerializeField] private EnergyQuantityData from;
        [SerializeField] private EnergyQuantityData to;

        public EnergyQuantityData From => from;
        public EnergyQuantityData To => to;

        #region Editor
        #if UNITY_EDITOR
        public void EditFrom(EnergyQuantityData newFrom) => from = newFrom;
        public void EditTo(EnergyQuantityData newTo) => to = newTo;
        #endif
        #endregion
    }


    [Serializable]
    public class EnergyStrengthModification
    {
        [SerializeField] private EnergyQuantityData from;
        [SerializeField] private EnergyModificationType modificationType;

        public EnergyQuantityData From => from;
        public EnergyModificationType ModificationType => modificationType;

        #region Editor
        #if UNITY_EDITOR
        public void EditFrom(EnergyQuantityData newFrom) => from = newFrom;
        public void EditTo(EnergyModificationType newModificationType) => modificationType = newModificationType;
        #endif
        #endregion
    }

    [Serializable]
    public class RemainingTurnsModification
    {
        [SerializeField] private RemainingTurnsModificationType type;
        [Range(1,10)] [SerializeField] private int value;

        public RemainingTurnsModificationType Type => type;
        public int Value => value;

        #region Editor
        #if UNITY_EDITOR
        public void EditType(RemainingTurnsModificationType newType) => type = newType;
        public void EditValue(int newValue) => value = newValue;
        #endif
        #endregion
    }

    [Serializable]
    public class CardDescriptionData
    {
        [Header("Text")]
        [SerializeField] private string descriptionText;
        [SerializeField] private bool enableOverrideColor;
        [SerializeField] private Color overrideColor = Color.black;
       
        [Header("Modifer")]
        [SerializeField] private bool useModifier;
        [SerializeField] private int modifiedActionValueIndex;
        [SerializeField] private StatusType modiferStats;
        [SerializeField] private bool usePrefixOnModifiedValue;
        [SerializeField] private string modifiedValuePrefix = "*";
        [SerializeField] private bool overrideColorOnValueScaled;

        public string DescriptionText => descriptionText;
        public bool EnableOverrideColor => enableOverrideColor;
        public Color OverrideColor => overrideColor;
        public bool UseModifier => useModifier;
        public int ModifiedActionValueIndex => modifiedActionValueIndex;
        public StatusType ModiferStats => modiferStats;
        public bool UsePrefixOnModifiedValue => usePrefixOnModifiedValue;
        public string ModifiedValuePrefix => modifiedValuePrefix;
        public bool OverrideColorOnValueScaled => overrideColorOnValueScaled;
        
        private CombatManager CombatManager => CombatManager.Instance;

        public string GetDescription()
        {
            var str = new StringBuilder();
            
            str.Append(DescriptionText);
            
            if (EnableOverrideColor && !string.IsNullOrEmpty(str.ToString())) 
                str.Replace(str.ToString(),ColorExtentions.ColorString(str.ToString(),OverrideColor));
            
            return str.ToString();
        }

        public string GetModifiedValue(CardData cardData)
        {
            if (cardData.CardActionDataList.Count <= 0) return "";
            
            if (ModifiedActionValueIndex>=cardData.CardActionDataList.Count)
                modifiedActionValueIndex = cardData.CardActionDataList.Count - 1;

            if (ModifiedActionValueIndex<0)
                modifiedActionValueIndex = 0;
            
            var str = new StringBuilder();
            var value = cardData.CardActionDataList[ModifiedActionValueIndex].ActionValue;
            var modifer = 0;
            if (CombatManager)
            {
                var player = CombatManager.CurrentMainAlly;
               
                if (player)
                {
                    modifer = player.CharacterStats.StatusDict[ModiferStats].StatusValue;
                    value += modifer;

                    if (modifer != 0)
                    {
                        if (usePrefixOnModifiedValue)
                            str.Append(modifiedValuePrefix);
                    }
                }
            }
           
            str.Append(value);

            if (EnableOverrideColor)
            {
                if (OverrideColorOnValueScaled)
                {
                    if (modifer != 0)
                        str.Replace(str.ToString(),ColorExtentions.ColorString(str.ToString(),OverrideColor));
                }
                else
                {
                    str.Replace(str.ToString(),ColorExtentions.ColorString(str.ToString(),OverrideColor));
                }
               
            }
            
            return str.ToString();
        }

        #region Editor
        #if UNITY_EDITOR
        
        public string GetDescriptionEditor()
        {
            var str = new StringBuilder();
            
            str.Append(DescriptionText);
            
            return str.ToString();
        }

        public string GetModifiedValueEditor(CardData cardData)
        {
            if (cardData.CardActionDataList.Count <= 0) return "";
            
            if (ModifiedActionValueIndex>=cardData.CardActionDataList.Count)
                modifiedActionValueIndex = cardData.CardActionDataList.Count - 1;

            if (ModifiedActionValueIndex<0)
                modifiedActionValueIndex = 0;
            
            var str = new StringBuilder();
            var value = cardData.CardActionDataList[ModifiedActionValueIndex].ActionValue;
            if (CombatManager)
            {
                var player = CombatManager.CurrentMainAlly;
                if (player)
                {
                    var modifer =player.CharacterStats.StatusDict[ModiferStats].StatusValue;
                    value += modifer;
                
                    if (modifer!= 0)
                        str.Append("*");
                }
            }
           
            str.Append(value);
          
            return str.ToString();
        }
        
        public void EditDescriptionText(string newText) => descriptionText = newText;
        public void EditEnableOverrideColor(bool newStatus) => enableOverrideColor = newStatus;
        public void EditOverrideColor(Color newColor) => overrideColor = newColor;
        public void EditUseModifier(bool newStatus) => useModifier = newStatus;
        public void EditModifiedActionValueIndex(int newIndex) => modifiedActionValueIndex = newIndex;
        public void EditModiferStats(StatusType newStatusType) => modiferStats = newStatusType;
        public void EditUsePrefixOnModifiedValues(bool newStatus) => usePrefixOnModifiedValue = newStatus;
        public void EditPrefixOnModifiedValues(string newText) => modifiedValuePrefix = newText;
        public void EditOverrideColorOnValueScaled(bool newStatus) => overrideColorOnValueScaled = newStatus;

        #endif
        #endregion
    }
}