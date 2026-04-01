using System.Collections.Generic;
using System.Linq;
using NueGames.NueDeck.Scripts.Card;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Data.Containers;
using NueGames.NueDeck.Scripts.Data.Settings;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Floors;
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
        protected AudioManager AudioManager => AudioManager.Instance;
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
                InitGameplayData();
                SetInitialHand();
            }
        }
        public void Start()
        {
          StartBGM();  
        }
        #endregion

        #region Public Methods
        public void InitGameplayData()
        { 
            PersistentGameplayData = new PersistentGameplayData(gameplayData);
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

            PlayerDeckData usedDeck = GameplayData.AvailableDecks.First(x => x.Floor == PersistentGameplayData.CurrentFloor);
            
            foreach (var cardData in usedDeck.CardList)
                PersistentGameplayData.CurrentCardsList.Add(cardData);
        }
        public void StartBGM()
        {
            AudioManager.PlayMusic(AudioActionType.MenuMusic);
        }
        public void ModifyRemainingTurns(int value, RemainingTurnsModificationType type)
        {
            int modifierValue = type == RemainingTurnsModificationType.Increase ? value : value * -1;
            PersistentGameplayData.RemainingActiveTurns += modifierValue;
            
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
