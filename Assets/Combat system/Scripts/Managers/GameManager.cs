using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Card;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Data.Containers;
using NueGames.NueDeck.Scripts.Data.Settings;
using NueGames.NueDeck.Scripts.EnemyBehaviour;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Floors;
using NueGames.NueDeck.Scripts.NueExtentions;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Managers
{
    [DefaultExecutionOrder(-10)]
    public class GameManager : MonoBehaviour
    { 
        public GameManager(){}
        public static GameManager Instance { get; private set; }
        
        [Header("Settings")]
        [SerializeField] private GameplayData gameplayData;
        [SerializeField] private List<EncounterData> encounterData;
        [SerializeField] private SceneData sceneData;

        #region Cache
        public SceneData SceneData => sceneData;
        public List<EncounterData> EncounterData => encounterData;
        public GameplayData GameplayData => gameplayData;
        public PersistentGameplayData PersistentGameplayData { get; private set; }
        protected UIManager UIManager => UIManager.Instance;
        #endregion
        
        #region Setup
        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
               
            }
            else
            {
                transform.parent = null;
                Instance = this;
                DontDestroyOnLoad(gameObject);
                CardActionProcessor.Initialize();
                EnemyActionProcessor.Initialize();
                InitGameplayData();
                SetInitialHand();
            }
        }
        #endregion
        
        #region Public Methods
        public void InitGameplayData()
        { 
            PersistentGameplayData = new PersistentGameplayData(gameplayData);
            if (UIManager)
                UIManager.InformationCanvas.ResetCanvas();
           
        } 
        public CardBase BuildAndGetCard(CardData targetData, Transform parent)
        {
            var clone = Instantiate(GameplayData.CardPrefab, parent);
            clone.SetCard(targetData);
            return clone;
        }
        public void SetInitialHand()
        {
            PersistentGameplayData.CurrentCardsList.Clear();
            
            foreach (var cardData in GameplayData.InitialDeck.CardList)
                PersistentGameplayData.CurrentCardsList.Add(cardData);
        }
        public void ModifyRemainingTurns(int value, RemainingTurnsModificationType type)
        {
            int modifierValue = type == RemainingTurnsModificationType.Increase ? value : value*-1;
            PersistentGameplayData.RemainingActiveTurns += modifierValue;

            Debug.Log(PersistentGameplayData.RemainingActiveTurns);
        }
        public void NextFloor()
        {
            PersistentGameplayData.CurrentFloor = FloorIdHelper.GetNewFloorId(PersistentGameplayData.CurrentFloor, FloorDirection.Up);   
        }
        public void OnExitApp()
        {
            
        }
        #endregion
      

    }
}
