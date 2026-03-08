using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Data.Energy;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.NueExtentions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace NueGames.NueDeck.Editor
{
    public class CardEditorWindow : ExtendedEditorWindow
    {
    #if UNITY_EDITOR
        

        private static CardEditorWindow CurrentWindow { get; set; }
        private SerializedObject _serializedObject;

        private const string CardDataDefaultPath = "Assets/NueGames/NueDeck/Data/Cards/";
       
        #region Cache Card Data
        private static CardData CachedCardData { get; set; }
        private List<CardData> AllCardDataList { get; set; }
        private CardData SelectedCardData { get; set; }
        private string CardId { get; set; }
        private string CardName { get; set; }
        private Sprite CardSprite{ get; set; }
        private bool UsableWithoutTarget{ get; set; }
        private List<CardActionData> CardActionDataList{ get; set; }
        private List<EnergyCardActionData> CardEnergyActionDataList { get; set; }
        private List<CardDescriptionData> CardDescriptionDataList{ get; set; }
        private List<SpecialKeywords> SpecialKeywordsList{ get; set; }
        private AudioActionType AudioType{ get; set; }
        
        private RarityType CardRarity { get; set; }

        private void CacheCardData()
        {
            CardId = SelectedCardData.Id;
            CardName = SelectedCardData.CardName;
            CardSprite = SelectedCardData.CardSprite;
            UsableWithoutTarget = SelectedCardData.UsableWithoutTarget;
            CardActionDataList = SelectedCardData.CardActionDataList.Count>0 ? new List<CardActionData>(SelectedCardData.CardActionDataList) : new List<CardActionData>();
            CardEnergyActionDataList = SelectedCardData.CardEnergyActionDataList.Count>0 ? new List<EnergyCardActionData>(SelectedCardData.CardEnergyActionDataList) : new List<EnergyCardActionData>();
            CardDescriptionDataList = SelectedCardData.CardDescriptionDataList.Count>0 ? new List<CardDescriptionData>(SelectedCardData.CardDescriptionDataList) : new List<CardDescriptionData>();
            SpecialKeywordsList = SelectedCardData.KeywordsList.Count>0 ? new List<SpecialKeywords>(SelectedCardData.KeywordsList) : new List<SpecialKeywords>();
            AudioType = SelectedCardData.AudioType;
            CardRarity = SelectedCardData.Rarity;
        }
        
        private void ClearCachedCardData()
        {
            CardId = String.Empty;
            CardName = String.Empty;
            CardSprite = null;
            UsableWithoutTarget = false;
            CardActionDataList?.Clear();
            CardEnergyActionDataList?.Clear();
            CardDescriptionDataList?.Clear();
            SpecialKeywordsList?.Clear();
            AudioType = AudioActionType.Attack;
            CardRarity = RarityType.Common;
        }
        #endregion
        
        #region Setup
        [MenuItem("Tools/NueDeck/Card Editor")]
        public static void OpenCardEditor() =>  CurrentWindow = GetWindow<CardEditorWindow>("Card Editor");
        public static void OpenCardEditor(CardData targetData)
        {
            CachedCardData = targetData;
            OpenCardEditor();
        } 
        
        private void OnEnable()
        {
            AllCardDataList?.Clear();
            AllCardDataList = ListExtentions.GetAllInstances<CardData>().ToList();
            
            if (CachedCardData)
            {
                SelectedCardData = CachedCardData;
                _serializedObject = new SerializedObject(SelectedCardData);
                CacheCardData();
            }
            
            Selection.selectionChanged += Repaint;
        }

        private void OnDisable()
        {
            Selection.selectionChanged -= Repaint;
            CachedCardData = null;
            SelectedCardData = null;
        }
        #endregion

        #region Process
        void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            
            DrawAllCardButtons();
            EditorGUILayout.Space();
            DrawSelectedCard();
            
            EditorGUILayout.EndHorizontal();
        }
        #endregion
        
        #region Layout Methods
        private Vector2 _allCardButtonsScrollPos;
        private void DrawAllCardButtons()
        {
            _allCardButtonsScrollPos = EditorGUILayout.BeginScrollView(_allCardButtonsScrollPos, GUILayout.Width(150), GUILayout.ExpandHeight(true));
            EditorGUILayout.BeginVertical("box", GUILayout.Width(150), GUILayout.ExpandHeight(true));
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Cards",EditorStyles.boldLabel,GUILayout.Width(50),GUILayout.Height(20));
            
            GUILayout.FlexibleSpace();
            
            var oldColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.blue;
            if (GUILayout.Button("Refresh",GUILayout.Width(75),GUILayout.Height(20)))
                RefreshCardData();
            GUI.backgroundColor = oldColor;
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Separator();

            foreach (var data in AllCardDataList)
                if (GUILayout.Button(data.CardName,GUILayout.MaxWidth(150)))
                {
                    SelectedCardData = data;
                    _serializedObject = new SerializedObject(SelectedCardData);
                    CacheCardData();
                    GUI.FocusControl(null);
                }

            if (GUILayout.Button("+",GUILayout.MaxWidth(150)))
            {
                CreateNewCard();
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }

        private void CreateNewCard()
        {
            var clone = CreateInstance<CardData>();
            var str = new StringBuilder();
            var count = AllCardDataList.Count;

            str.Append(count + 1).Append("_").Append("new_card_name");
            clone.EditId(str.ToString());
            clone.EditCardName(str.ToString());
            clone.EditCardActionDataList(new List<CardActionData>());
            clone.EditCardEnergyActionDataList(new List<EnergyCardActionData>());
            clone.EditCardDescriptionDataList(new List<CardDescriptionData>());
            clone.EditSpecialKeywordsList(new List<SpecialKeywords>());
            clone.EditRarity(RarityType.Common);
            var path = str.Insert(0, CardDataDefaultPath).Append(".asset").ToString();
            var uniquePath = AssetDatabase.GenerateUniqueAssetPath(path);
            AssetDatabase.CreateAsset(clone, uniquePath);
            AssetDatabase.SaveAssets();
            RefreshCardData();
            SelectedCardData = AllCardDataList.Find(x => x.Id == clone.Id);
            CacheCardData();
        }

        private void DrawSelectedCard()
        {
            EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
            if (!SelectedCardData)
            {
                EditorGUILayout.LabelField("Select card");
                EditorGUILayout.EndVertical();
                return;
            }
            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
           
            ChangeGeneralSettings();

            ChangeCardActionDataList();
            ChangeEnergyActionsDataList();
            
            ChangeCardDescriptionDataList();
            ChangeSpecialKeywords();
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            var oldColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Save",GUILayout.Width(100),GUILayout.Height(30)))
                SaveCardData();
            
            GUI.backgroundColor = oldColor;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
        #endregion

        #region Card Data Methods
        private bool _isGeneralSettingsFolded;
        private Vector2 _generalSettingsScrollPos;
        private void ChangeGeneralSettings()
        {
            _isGeneralSettingsFolded =EditorGUILayout.BeginFoldoutHeaderGroup(_isGeneralSettingsFolded, "General Settings");
            if (!_isGeneralSettingsFolded)
            {
                EditorGUILayout.EndFoldoutHeaderGroup();
                return;
            }
            ChangeId();
            ChangeCardName();
            _generalSettingsScrollPos = EditorGUILayout.BeginScrollView(_generalSettingsScrollPos,GUILayout.ExpandWidth(true));
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            ChangeRarity();
            ChangeUsableWithoutTarget();
            ChangeAudioActionType();
            EditorGUILayout.EndVertical();
            GUILayout.Space(100);
            ChangeCardSprite();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
        private void ChangeId()
        {
            CardId = EditorGUILayout.TextField("Card Id:", CardId);
        }
        private void ChangeCardName()
        {
            CardName = EditorGUILayout.TextField("Card Name:", CardName);
        }
         private void ChangeRarity()
        { 
            CardRarity = (RarityType) EditorGUILayout.EnumPopup("Rarity: ",CardRarity,GUILayout.Width(250));
        }
        private void ChangeCardSprite()
        {
            EditorGUILayout.BeginHorizontal();
            CardSprite = (Sprite)EditorGUILayout.ObjectField(
                "Card Sprite:",
                CardSprite,
                typeof(Sprite),
                false
            );
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
        private void ChangeUsableWithoutTarget()
        {
            UsableWithoutTarget = EditorGUILayout.Toggle("Usable Without Target:", UsableWithoutTarget);
        }
        private bool _isCardActionDataListFolded;
        private Vector2 _cardActionScrollPos;
        private void ChangeCardActionDataList()
        {
            _isCardActionDataListFolded =EditorGUILayout.BeginFoldoutHeaderGroup(_isCardActionDataListFolded, "Card Actions");
            if (_isCardActionDataListFolded)
            {
                _cardActionScrollPos = EditorGUILayout.BeginScrollView(_cardActionScrollPos,GUILayout.ExpandWidth(true));
                EditorGUILayout.BeginHorizontal();
                List<CardActionData> _removedList = new List<CardActionData>();
                for (var i = 0; i < CardActionDataList.Count; i++)
                {
                    var cardActionData = CardActionDataList[i];
                    EditorGUILayout.BeginVertical("box", GUILayout.Width(150), GUILayout.MaxHeight(50));
                
                    EditorGUILayout.BeginHorizontal();
                    GUIStyle idStyle = new GUIStyle();
                    idStyle.fontSize = 16;
                    idStyle.fixedWidth = 25;
                    idStyle.fixedHeight = 25;
                    idStyle.fontStyle = FontStyle.Bold;
                    idStyle.normal.textColor = Color.white;
                    EditorGUILayout.LabelField($"Action Index: {i}",idStyle);
                    
                    GUILayout.FlexibleSpace();
                    
                    if (GUILayout.Button("X", GUILayout.MaxWidth(25), GUILayout.MaxHeight(25)))
                        _removedList.Add(cardActionData);
                    
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Separator();
                    var newActionType = (CardActionType)EditorGUILayout.EnumPopup("Action Type",cardActionData.CardActionType,GUILayout.Width(250));

                    var newActionTarget = (ActionTargetType)EditorGUILayout.EnumPopup("Target Type",cardActionData.ActionTargetType,GUILayout.Width(250));
                    var newActionValue = EditorGUILayout.FloatField("Action Value: ",cardActionData.ActionValue);
                    var newActionDelay = EditorGUILayout.FloatField("Action Delay: ",cardActionData.ActionDelay);
                    var newUsableWithoutCost = EditorGUILayout.Toggle("Usable Without Cost:", cardActionData.UsableWithoutCost);
                    var newOptional = EditorGUILayout.Toggle("Optional:", cardActionData.Optional);
                    EditorGUILayout.LabelField("Cost", EditorStyles.boldLabel);
                    cardActionData.EditCostDataList(DrawEnergyQuantityList(cardActionData.CostDataList));

                    cardActionData.EditOptional(newOptional);
                    cardActionData.EditUsableWithoutCost(newUsableWithoutCost);
                    cardActionData.EditActionValue(newActionValue);
                    cardActionData.EditActionTarget(newActionTarget);
                    cardActionData.EditActionDelay(newActionDelay);
                    
                    cardActionData.EditActionType(newActionType);
                    EditorGUILayout.EndVertical();
                }

                foreach (var cardActionData in _removedList)
                    CardActionDataList.Remove(cardActionData);

                if (GUILayout.Button("+",GUILayout.Width(50),GUILayout.Height(50)))
                    CardActionDataList.Add(new CardActionData());
                
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndScrollView();
            }
            
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
        private bool _isEnergyActionsDataListFolded;
        private Vector2 _energyActionsDataListScrollPos;
        private void ChangeEnergyActionsDataList()
        {
            _isEnergyActionsDataListFolded =EditorGUILayout.BeginFoldoutHeaderGroup(_isEnergyActionsDataListFolded, "Card energy actions");
            if (_isEnergyActionsDataListFolded)
            {
                _energyActionsDataListScrollPos = EditorGUILayout.BeginScrollView(_energyActionsDataListScrollPos,GUILayout.ExpandWidth(true));
                EditorGUILayout.BeginHorizontal();
                List<EnergyCardActionData> _removedList = new List<EnergyCardActionData>();
                for (var i = 0; i < CardEnergyActionDataList.Count; i++)
                {
                    EnergyCardActionData cardEnergyActionData = CardEnergyActionDataList[i];
                    EditorGUILayout.BeginVertical("box", GUILayout.Width(150), GUILayout.MaxHeight(50));
                    EditorGUILayout.BeginHorizontal();
                    GUIStyle idStyle = new GUIStyle();
                    idStyle.fontSize = 16;
                    idStyle.fixedWidth = 25;
                    idStyle.fixedHeight = 25;
                    idStyle.fontStyle = FontStyle.Bold;
                    idStyle.normal.textColor = Color.white;
                    EditorGUILayout.LabelField($"Energy action Index: {i}",idStyle);
                    
                    GUILayout.FlexibleSpace();
                    
                    if (GUILayout.Button("X", GUILayout.MaxWidth(25), GUILayout.MaxHeight(25)))
                        _removedList.Add(cardEnergyActionData);
                    
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Separator();

                    EnergyCardActionType previousType = cardEnergyActionData.CardActionType;
                    EnergyCardActionType newEnergyActionType = (EnergyCardActionType)EditorGUILayout.EnumPopup("Action Type",cardEnergyActionData.CardActionType,GUILayout.Width(250));

                    bool newUsableWithoutCost = EditorGUILayout.Toggle("Usable Without Cost:", cardEnergyActionData.UsableWithoutCost);
                    bool newOptional = EditorGUILayout.Toggle("Optional:", cardEnergyActionData.Optional);
                    float newActionDelay = EditorGUILayout.Slider("Action Delay: ",cardEnergyActionData.ActionDelay, 0.1f, 10f);
                    

                    EditorGUILayout.LabelField("Cost", EditorStyles.boldLabel);
                    cardEnergyActionData.EditCostDataList(DrawEnergyQuantityList(cardEnergyActionData.CostDataList));

                    cardEnergyActionData.EditUsableWithoutCost(newUsableWithoutCost);
                    cardEnergyActionData.EditOptional(newOptional);
                    cardEnergyActionData.EditActionDelay(newActionDelay);

                    switch (newEnergyActionType)
                    {
                        case EnergyCardActionType.CreateEnergy:
                            DrawCreateEnergy(cardEnergyActionData);
                            break;
                        
                        case EnergyCardActionType.ConvertEnergy:
                            DrawConvertEnergy(cardEnergyActionData);
                            break;

                        case EnergyCardActionType.ModifyEnergyStrength:
                            DrawModifyEnergyStrength(cardEnergyActionData);
                            break;

                        case EnergyCardActionType.ModifyRemainingTurns:
                            DrawModifyRemainingTurns(cardEnergyActionData);
                            break;
                        
                        case EnergyCardActionType.BlockEnergyGeneration:
                            DrawBlockEnergyGeneration(cardEnergyActionData);
                            break;

                        case EnergyCardActionType.ModifyGenerationPool:
                            DrawModifyGenerationPool(cardEnergyActionData);
                            break;
                        
                        case EnergyCardActionType.BlockEnergyUsage:
                            DrawBlockEnergyUsage(cardEnergyActionData);
                            break;
                    }
                    
                    if (newEnergyActionType != previousType)
                    {
                        CleanEnergyActions(cardEnergyActionData);
                        cardEnergyActionData.EditActionType(newEnergyActionType);
                    }

                    EditorGUILayout.EndVertical();
                }

                foreach (var cardEnergyActionData in _removedList)
                    CardEnergyActionDataList.Remove(cardEnergyActionData);

                if (GUILayout.Button("+",GUILayout.Width(50),GUILayout.Height(50)))
                    CardEnergyActionDataList.Add(new EnergyCardActionData());
                
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndScrollView();
            }
            
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
        private EnergyCardActionData CleanEnergyActions(EnergyCardActionData cardEnergyActionData)
        {
            cardEnergyActionData.EditEnergyToCreate(null);
            cardEnergyActionData.EditEnergyToConvert(null);
            cardEnergyActionData.EditEnergyToModifyStrength(null);
            cardEnergyActionData.EditTurnsModification(null);
            cardEnergyActionData.EditBlockEnergyGeneration(null);
            cardEnergyActionData.EditModifyEnergyGenerationPool(null);
            cardEnergyActionData.EditBlockEnergyUsage(null);
            return cardEnergyActionData;
        }
        private void DrawCreateEnergy(EnergyCardActionData cardEnergyActionData)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Energy To Create", EditorStyles.boldLabel);
        
            cardEnergyActionData.EditEnergyToCreate(DrawEnergyQuantityList(cardEnergyActionData.EnergyToCreate));
        }
        private void DrawConvertEnergy(EnergyCardActionData cardEnergyActionData)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Energy To Convert", EditorStyles.boldLabel);
        
            if (cardEnergyActionData.EnergyToConvert == null)
                cardEnergyActionData.EditEnergyToConvert(new List<EnergyConversion>());
        
            List<EnergyConversion> _removeConversionList = new List<EnergyConversion>();
        
            for (int i = 0; i < cardEnergyActionData.EnergyToConvert.Count; i++)
            {
                EnergyConversion energyConversionData = cardEnergyActionData.EnergyToConvert[i];
        
                EditorGUILayout.BeginHorizontal("box");
                EditorGUILayout.BeginHorizontal();
        
                EditorGUILayout.BeginVertical();

                EditorGUILayout.LabelField("From",EditorStyles.boldLabel, GUILayout.Width(100));
                EnergyQuantityData newFrom = DrawEnergyQuantitySingle(energyConversionData.From);

                EditorGUILayout.LabelField("To",EditorStyles.boldLabel, GUILayout.Width(100));
                EnergyQuantityData newTo = DrawEnergyQuantitySingle(energyConversionData.To);

                EditorGUILayout.EndVertical(); 

                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    _removeConversionList.Add(energyConversionData);
                }

                EditorGUILayout.EndHorizontal();

                energyConversionData.EditFrom(newFrom);
                energyConversionData.EditTo(newTo);
        
                EditorGUILayout.EndHorizontal();
            }

            foreach (var energy in _removeConversionList)
                cardEnergyActionData.EnergyToConvert.Remove(energy);

            if (GUILayout.Button("+"))
                cardEnergyActionData.EnergyToConvert.Add(new EnergyConversion());
        }
        private void DrawModifyEnergyStrength(EnergyCardActionData cardEnergyActionData)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Energy To Modify Strength", EditorStyles.boldLabel);
        
            if (cardEnergyActionData.EnergyToModifyStrength == null)
                cardEnergyActionData.EditEnergyToModifyStrength(new List<EnergyStrengthModification>());
        
            List<EnergyStrengthModification> removeEnergyList = new List<EnergyStrengthModification>();
        
            for (int i = 0; i < cardEnergyActionData.EnergyToModifyStrength.Count; i++)
            {
                var energyStrengthModificationData = cardEnergyActionData.EnergyToModifyStrength[i];
        
                EditorGUILayout.BeginHorizontal("box");
                EditorGUILayout.BeginHorizontal();
        
                EditorGUILayout.BeginVertical();

                EditorGUILayout.LabelField("From",EditorStyles.boldLabel, GUILayout.Width(100));
                EnergyQuantityData newFrom = DrawEnergyQuantitySingle(energyStrengthModificationData.From);

                EditorGUILayout.LabelField("Modification type",EditorStyles.boldLabel, GUILayout.Width(100));
                EnergyModificationType newTo = (EnergyModificationType)EditorGUILayout.EnumPopup(energyStrengthModificationData.ModificationType);
            
                EditorGUILayout.EndVertical(); 
        
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    removeEnergyList.Add(energyStrengthModificationData);
                }
                    
                EditorGUILayout.EndHorizontal();

                energyStrengthModificationData.EditFrom(newFrom);
                energyStrengthModificationData.EditTo(newTo);
        
                EditorGUILayout.EndHorizontal();
            }

            foreach (var energy in removeEnergyList)
                cardEnergyActionData.EnergyToModifyStrength.Remove(energy);

            if (GUILayout.Button("+"))
                cardEnergyActionData.EnergyToModifyStrength.Add(new EnergyStrengthModification());
        }
        private List<EnergyQuantityData> DrawEnergyQuantityList(List<EnergyQuantityData> energyQuantityDataList)
        {        
            if (energyQuantityDataList == null)
                energyQuantityDataList = new List<EnergyQuantityData>();
        
            List<EnergyQuantityData> removeEnergyList = new();
        
            for (int i = 0; i < energyQuantityDataList.Count; i++)
            {
                GUILayout.BeginHorizontal("Box");
                var energyQuantityData = energyQuantityDataList[i];
                energyQuantityData = DrawEnergyQuantitySingle(energyQuantityData);
                
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    removeEnergyList.Add(energyQuantityData);
                }
                GUILayout.EndHorizontal();
            }

            foreach (var energy in removeEnergyList)
                energyQuantityDataList.Remove(energy);

            if (GUILayout.Button("+"))
                energyQuantityDataList.Add(new EnergyQuantityData());

            return energyQuantityDataList;
        }
        private EnergyQuantityData DrawEnergyQuantitySingle(EnergyQuantityData energyQuantityData)
        {
            if (energyQuantityData == null)
                energyQuantityData = new EnergyQuantityData();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Energy", GUILayout.Width(70));
            var newEnergyType = (EnergyColor)EditorGUILayout.EnumPopup(energyQuantityData.EnergyColor);     
            var newEnergyQuantity = EditorGUILayout.IntSlider("Quantity", energyQuantityData.Quantity, 1, 10);
            EditorGUILayout.EndHorizontal();
            
            energyQuantityData.EditCostType(newEnergyType);
            energyQuantityData.EditQuantityCost(newEnergyQuantity);

            return energyQuantityData;
        } 
        private void DrawModifyRemainingTurns(EnergyCardActionData cardEnergyActionData)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Modify turns data", EditorStyles.boldLabel);
            RemainingTurnsModification remainingTurnsModification = cardEnergyActionData.TurnsModification;
            
            if (remainingTurnsModification is null)
            {
                remainingTurnsModification = new RemainingTurnsModification();
                cardEnergyActionData.EditTurnsModification(remainingTurnsModification);
            }

            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.BeginHorizontal();
        
            EditorGUILayout.BeginVertical();

            EditorGUILayout.LabelField("Type",EditorStyles.boldLabel, GUILayout.Width(100));
            RemainingTurnsModificationType newType = (RemainingTurnsModificationType)EditorGUILayout.EnumPopup(remainingTurnsModification.Type);

            EditorGUILayout.LabelField("Value",EditorStyles.boldLabel, GUILayout.Width(100));
            int newValue = EditorGUILayout.IntSlider(remainingTurnsModification.Value, 1, 10);
                
            EditorGUILayout.EndVertical(); 

            EditorGUILayout.EndHorizontal();

            remainingTurnsModification.EditValue(newValue);
            remainingTurnsModification.EditType(newType);
        
            EditorGUILayout.EndHorizontal();
        }
        private void DrawBlockEnergyGeneration(EnergyCardActionData cardEnergyActionData)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Modify turns data", EditorStyles.boldLabel);
            BlockEnergyGeneration blockEnergyGeneration = cardEnergyActionData.BlockEnergyGeneration;

            if (blockEnergyGeneration is null)
            {
                blockEnergyGeneration = new BlockEnergyGeneration();
                cardEnergyActionData.EditBlockEnergyGeneration(blockEnergyGeneration);
            }

            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.BeginHorizontal();
        
            EditorGUILayout.BeginVertical();

            EditorGUILayout.LabelField("Turns",EditorStyles.boldLabel, GUILayout.Width(100));
            int newTurns = EditorGUILayout.IntSlider(blockEnergyGeneration.Turns, 1, 10);

            EditorGUILayout.EndVertical(); 

            EditorGUILayout.EndHorizontal();

            blockEnergyGeneration.EditTurns(newTurns);
        
            EditorGUILayout.EndHorizontal();
        }
        private void DrawModifyGenerationPool(EnergyCardActionData cardEnergyActionData)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Modify turns data", EditorStyles.boldLabel);
            ModifyEnergyGenerationPool modifyEnergyGenerationPool = cardEnergyActionData.ModifyEnergyGenerationPool;
            
            if (modifyEnergyGenerationPool is null)
            {
                modifyEnergyGenerationPool = new ModifyEnergyGenerationPool();
                cardEnergyActionData.EditModifyEnergyGenerationPool(modifyEnergyGenerationPool);
            }
        
            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.BeginHorizontal();
        
            EditorGUILayout.BeginVertical();

            EditorGUILayout.LabelField("Turns",EditorStyles.boldLabel, GUILayout.Width(100));
            int newTurns = EditorGUILayout.IntSlider(modifyEnergyGenerationPool.Turns, 1, 10);

            EditorGUILayout.LabelField("Max energies spawn",EditorStyles.boldLabel, GUILayout.Width(150));
            int newMaxEnergiesSpawn = EditorGUILayout.IntSlider(modifyEnergyGenerationPool.MaxEnergiesSpawn, 1, 10);

            EditorGUILayout.LabelField("Min energies spawn",EditorStyles.boldLabel, GUILayout.Width(150));
            int newMinEnergiesSpawn = EditorGUILayout.IntSlider(modifyEnergyGenerationPool.MinEnergiesSpawn, 1, 10);
    
            EditorGUILayout.LabelField("Available Energies", EditorStyles.boldLabel);

            List<EnergyData> newAvailableEnergies = modifyEnergyGenerationPool.AvailableEnergies;

            if (newAvailableEnergies is null)
            {
                newAvailableEnergies = new();
                modifyEnergyGenerationPool.EditAvailableEnergies(newAvailableEnergies);
            }

            for (int i = 0; i < newAvailableEnergies.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                newAvailableEnergies[i] = (EnergyData)EditorGUILayout.ObjectField(
                    newAvailableEnergies[i],
                    typeof(EnergyData),
                    false
                );

                if (GUILayout.Button("x", GUILayout.Width(25)))
                {
                    newAvailableEnergies.RemoveAt(i);
                }

                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("+"))
            {
                newAvailableEnergies.Add(null);
            }

            EditorGUILayout.EndVertical(); 

            EditorGUILayout.EndHorizontal();

            modifyEnergyGenerationPool.EditTurns(newTurns);
            modifyEnergyGenerationPool.EditMaxEnergiesSpawn(newMaxEnergiesSpawn);
            modifyEnergyGenerationPool.EditMinEnergiesSpawn(newMinEnergiesSpawn);
            modifyEnergyGenerationPool.EditAvailableEnergies(newAvailableEnergies);
        
            EditorGUILayout.EndHorizontal();
        }
        private void DrawBlockEnergyUsage(EnergyCardActionData cardEnergyActionData)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Block energy usage", EditorStyles.boldLabel);
            BlockEnergyUsage blockEnergyUsage = cardEnergyActionData.BlockEnergyUsage;

            if (blockEnergyUsage is null)
            {
                blockEnergyUsage = new BlockEnergyUsage();
                cardEnergyActionData.EditBlockEnergyUsage(blockEnergyUsage);
            }

            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.BeginHorizontal();
        
            EditorGUILayout.BeginVertical();

            EditorGUILayout.LabelField("Turns",EditorStyles.boldLabel, GUILayout.Width(100));
            int newTurns = EditorGUILayout.IntSlider(blockEnergyUsage.Turns, 1, 10);

            EditorGUILayout.LabelField("Color",EditorStyles.boldLabel, GUILayout.Width(100));
            EnergyColor newColor = (EnergyColor)EditorGUILayout.EnumPopup(blockEnergyUsage.Color);

            EditorGUILayout.EndVertical(); 

            EditorGUILayout.EndHorizontal();

            blockEnergyUsage.EditTurns(newTurns);
            blockEnergyUsage.EditColor(newColor);
        
            EditorGUILayout.EndHorizontal();
        }
        private bool _isDescriptionDataListFolded;
        private Vector2 _descriptionDataScrollPos;
        private void ChangeCardDescriptionDataList()
        {
            _isDescriptionDataListFolded =EditorGUILayout.BeginFoldoutHeaderGroup(_isDescriptionDataListFolded, "Description");
            if (_isDescriptionDataListFolded)
            {
                _descriptionDataScrollPos = EditorGUILayout.BeginScrollView(_descriptionDataScrollPos,GUILayout.ExpandWidth(true));
                EditorGUILayout.BeginHorizontal();
                List<CardDescriptionData> _removedList = new List<CardDescriptionData>();
                for (var i = 0; i < CardDescriptionDataList.Count; i++)
                {
                    var descriptionData = CardDescriptionDataList[i];
                    
                    EditorGUILayout.BeginVertical("box", GUILayout.Width(175), GUILayout.Height(100));
                    EditorGUILayout.BeginHorizontal();
                    descriptionData.EditUseModifier(EditorGUILayout.ToggleLeft("Use Modifier", descriptionData.UseModifier,
                        GUILayout.Width(125), GUILayout.Height(25)));
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("X", GUILayout.Width(25), GUILayout.Height(25)))
                        _removedList.Add(descriptionData);
                    EditorGUILayout.EndHorizontal();
                    
                    descriptionData.EditEnableOverrideColor(EditorGUILayout.ToggleLeft("Override Color", descriptionData.EnableOverrideColor,
                        GUILayout.Width(125), GUILayout.Height(25)));
                    
                    EditorGUILayout.Space(5);

                    if (descriptionData.EnableOverrideColor)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.BeginVertical();
                        EditorGUILayout.Separator();
                        descriptionData.EditOverrideColor(EditorGUILayout.ColorField(descriptionData.OverrideColor));
                        descriptionData.EditOverrideColorOnValueScaled(EditorGUILayout.ToggleLeft("Color on scale", descriptionData.OverrideColorOnValueScaled,
                            GUILayout.Width(125), GUILayout.Height(25)));
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space(5);
                    }
                    
                    EditorGUILayout.BeginHorizontal();
                    if (descriptionData.UseModifier)
                    {
                        EditorGUILayout.BeginVertical();
                        
                        var clampedIndex = Mathf.Clamp(descriptionData.ModifiedActionValueIndex, 0,
                            CardActionDataList.Count - 1);
                        descriptionData.EditModifiedActionValueIndex(
                            EditorGUILayout.IntField("Action Index:",clampedIndex));
                       
                        descriptionData.EditModiferStats((StatusType)EditorGUILayout.EnumPopup("Scale Type:",descriptionData.ModiferStats));
                        descriptionData.EditUsePrefixOnModifiedValues(EditorGUILayout.ToggleLeft("Use prefix on scale", descriptionData.UsePrefixOnModifiedValue,
                            GUILayout.Width(125), GUILayout.Height(25)));
                        if (descriptionData.UsePrefixOnModifiedValue)
                            descriptionData.EditPrefixOnModifiedValues(
                                EditorGUILayout.TextField("Prefix:",descriptionData.ModifiedValuePrefix));
                        
                        EditorGUILayout.EndVertical();
                    }
                    else
                    { 
                        var desc = EditorGUILayout.TextArea(descriptionData.DescriptionText, GUILayout.Width(150),
                            GUILayout.Height(50));

                        descriptionData.EditDescriptionText(desc);
                    }
                    
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                }
                
                

                foreach (var cardActionData in _removedList)
                    CardDescriptionDataList.Remove(cardActionData);

                if (GUILayout.Button("+",GUILayout.Width(50),GUILayout.Height(50)))
                    CardDescriptionDataList.Add(new CardDescriptionData());
                
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndScrollView();
                
                EditorGUILayout.Separator();
                EditorGUILayout.BeginHorizontal("box");
                var str = new StringBuilder();
                foreach (var cardDescriptionData in CardDescriptionDataList)
                {
                    str.Append(cardDescriptionData.UseModifier
                        ? cardDescriptionData.GetModifiedValueEditor(SelectedCardData)
                        : cardDescriptionData.GetDescriptionEditor());
                }
                
                GUIStyle headStyle = new GUIStyle();
                headStyle.fontStyle = FontStyle.Bold;
                headStyle.normal.textColor = Color.white;
                EditorGUILayout.BeginVertical();
                
                EditorGUILayout.LabelField("Description Preview",headStyle);
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField(str.ToString());
                EditorGUILayout.Separator();

               

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }
            
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private Vector2 _specialKeywordScrool;
        private bool _isSpecialKeywordsFolded;
        private void ChangeSpecialKeywords()
        {
            _isSpecialKeywordsFolded =EditorGUILayout.BeginFoldoutHeaderGroup(_isSpecialKeywordsFolded, "Special Keywords");
            if (!_isSpecialKeywordsFolded)
            {
                EditorGUILayout.EndFoldoutHeaderGroup();
                return;
            }
           
            EditorGUILayout.BeginVertical("box");
            _specialKeywordScrool = EditorGUILayout.BeginScrollView(_specialKeywordScrool);
            EditorGUILayout.BeginHorizontal();
            var specialKeyCount = Enum.GetNames(typeof(SpecialKeywords));

            for (var i = 0; i < specialKeyCount.Length; i++)
            {
                EditorGUILayout.BeginVertical(GUILayout.Width(100));
                var hasKey = SpecialKeywordsList.Contains((SpecialKeywords)i);
                EditorGUILayout.LabelField(((SpecialKeywords)i).ToString());
                var newValue = EditorGUILayout.Toggle(hasKey);
                if (newValue)
                {
                    if (!SpecialKeywordsList.Contains((SpecialKeywords)i))
                        SpecialKeywordsList.Add((SpecialKeywords)i);
                }
                else
                {
                    if (SpecialKeywordsList.Contains((SpecialKeywords)i))
                        SpecialKeywordsList.Remove((SpecialKeywords)i);
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
        private void ChangeAudioActionType()
        {
            AudioType = (AudioActionType)EditorGUILayout.EnumPopup("Audio Type:",AudioType);
        }       
        private void SaveCardData()
        {
            if (!SelectedCardData) return;
            
            SelectedCardData.EditId(CardId);
            SelectedCardData.EditCardName(CardName);
            SelectedCardData.EditCardSprite(CardSprite);
            SelectedCardData.EditUsableWithoutTarget(UsableWithoutTarget);
            SelectedCardData.EditCardActionDataList(CardActionDataList);
            SelectedCardData.EditCardEnergyActionDataList(CardEnergyActionDataList);
            SelectedCardData.EditCardDescriptionDataList(CardDescriptionDataList);
            SelectedCardData.EditSpecialKeywordsList(SpecialKeywordsList);
            SelectedCardData.EditAudioType(AudioType);
            EditorUtility.SetDirty(SelectedCardData);
            AssetDatabase.SaveAssets();
        }
        private void RefreshCardData()
        {
            SelectedCardData = null;
            ClearCachedCardData();
            AllCardDataList?.Clear();
            AllCardDataList = ListExtentions.GetAllInstances<CardData>().ToList();
        }
        #endregion
    #endif
    }
}
