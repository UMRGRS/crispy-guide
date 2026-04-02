using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Characters.Enemies;
using NueGames.NueDeck.Scripts.Data.Characters;
using NueGames.NueDeck.Scripts.Data.Containers;
using NueGames.NueDeck.Scripts.Enums;
using Random = UnityEngine.Random;
using NueGames.NueDeck.Scripts.Utils;

namespace NueGames.NueDeck.Scripts.Managers
{
    public class CombatManager : MonoBehaviour
    {
        private CombatManager(){}
        public static CombatManager Instance { get; private set; }

        [Header("References")] 
        [SerializeField] private List<Transform> enemyPosList;
        [SerializeField] private List<Transform> allyPosList;
        
        #region Cache
        public List<EnemyBase> CurrentEnemiesList { get; private set; } = new List<EnemyBase>();
        public List<AllyBase> CurrentAlliesList { get; private set; }= new List<AllyBase>();

        public Action OnAllyStatusTrigger;
        public Action OnEnemyActionDeclaration;
        public Action OnEnemyStatusTrigger;
        public List<Transform> EnemyPosList => enemyPosList;
        public List<Transform> AllyPosList => allyPosList;
        public AllyBase CurrentMainAlly => CurrentAlliesList.Count>0 ? CurrentAlliesList[0] : null;

        public CombatStateType CurrentCombatStateType
        {
            get => _currentCombatStateType;
            private set
            {
                if (_currentCombatStateType == value)
                    return;

                _currentCombatStateType = value;
                ExecuteCombatState(value);
            }
        }
        
        private CombatStateType _currentCombatStateType;
        private FxManager FxManager => FxManager.Instance;
        private AudioManager AudioManager => AudioManager.Instance;
        private GameManager GameManager => GameManager.Instance;
        private UIManager UIManager => UIManager.Instance;
        private CollectionManager CollectionManager => CollectionManager.Instance;
        private EnergyPoolManager EnergyPoolManager => EnergyPoolManager.Instance;
        private ScoreManager ScoreManager => ScoreManager.Instance;

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
                Instance = this;
                CurrentCombatStateType = CombatStateType.PrepareCombat;
            }
        }

        private void Start()
        {
            StartCombat();
        }
        public void StartCombat()
        {
            SetInitialData();
            BuildEnemies();
            BuildAllies();
            
            CollectionManager.SetGameDeck();
            ScoreManager.ClearScore();
            
            UIManager.CombatCanvas.gameObject.SetActive(true);
            UIManager.CombatCanvas.SetTurnsLeft();

            CurrentCombatStateType = CombatStateType.TurnStart;
        }
        
        private void ExecuteCombatState(CombatStateType targetStateType)
        {
            switch (targetStateType)
            {
                case CombatStateType.PrepareCombat:
                    break;
                case CombatStateType.TurnStart:
                    TurnStart();
                    break;

                case CombatStateType.EnemyDeclaration:
                    EnemyActionsDeclaration();
                    break;

                case CombatStateType.AllyTurn:
                    AllyTurn();
                    break;

                case CombatStateType.EnemyTurn:
                    EnemyTurn();
                    break; 

                case CombatStateType.TurnEnd:
                    TurnEnd();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetStateType), targetStateType, null);
            }
        }
        #endregion

        #region Public Methods
        public void EndAllyTurn()
        {
            CurrentCombatStateType = CombatStateType.EnemyTurn;
        }
        public void EndStartOfTurn()
        {
            CurrentCombatStateType = CombatStateType.EnemyDeclaration;
        }
        public void OnAllyDeath(AllyBase targetAlly)
        {
            var targetAllyData = GameManager.PersistentGameplayData.AllyList.Find(x =>
                x.AllyCharacterData.CharacterID == targetAlly.AllyCharacterData.CharacterID);
            if (GameManager.PersistentGameplayData.AllyList.Count>1)
                GameManager.PersistentGameplayData.AllyList.Remove(targetAllyData);
            CurrentAlliesList.Remove(targetAlly);
            if (CurrentAlliesList.Count<=0)
                LoseCombat();
        }
        public void OnEnemyDeath(EnemyBase targetEnemy)
        {
            CurrentEnemiesList.Remove(targetEnemy);
            if (CurrentEnemiesList.Count<=0)
                WinCombat();
        }
        public void DeactivateCardHighlights()
        {
            foreach (var currentEnemy in CurrentEnemiesList)
                currentEnemy.EnemyCanvas.SetHighlight(false);

            foreach (var currentAlly in CurrentAlliesList)
                currentAlly.AllyCanvas.SetHighlight(false);
        }
        public void HighlightCardTarget(ActionTargetType targetTypeTargetType)
        {
            switch (targetTypeTargetType)
            {
                case ActionTargetType.Enemy:
                    foreach (var currentEnemy in CurrentEnemiesList)
                        currentEnemy.EnemyCanvas.SetHighlight(true);
                    break;
                case ActionTargetType.Ally:
                    foreach (var currentAlly in CurrentAlliesList)
                        currentAlly.AllyCanvas.SetHighlight(true);
                    break;
                case ActionTargetType.AllEnemies:
                    foreach (var currentEnemy in CurrentEnemiesList)
                        currentEnemy.EnemyCanvas.SetHighlight(true);
                    break;
                case ActionTargetType.AllAllies:
                    foreach (var currentAlly in CurrentAlliesList)
                        currentAlly.AllyCanvas.SetHighlight(true);
                    break;
                case ActionTargetType.RandomEnemy:
                    foreach (var currentEnemy in CurrentEnemiesList)
                        currentEnemy.EnemyCanvas.SetHighlight(true);
                    break;
                case ActionTargetType.RandomAlly:
                    foreach (var currentAlly in CurrentAlliesList)
                        currentAlly.AllyCanvas.SetHighlight(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetTypeTargetType), targetTypeTargetType, null);
            }
        }
        #endregion
        
        #region Private Methods
        private void SetInitialData()
        {
            EncounterData currentFloorData = GameManager.EncounterData.First(encounterData => encounterData.Floor == GameManager.PersistentGameplayData.CurrentFloor);
            GameManager.PersistentGameplayData.CurrentEncounter = currentFloorData.GetEnemyEncounter(isBoss: true);

            EnemyEncounter currentEncounter = GameManager.PersistentGameplayData.CurrentEncounter;
            GameManager.PersistentGameplayData.RemainingActiveTurns = currentEncounter.EnergyGenerationTurns;

            GameManager.PersistentGameplayData.EnergyGenerationRules = new Data.Settings.EnergyGenerationRules();
            GameManager.PersistentGameplayData.EnergyBlockRules = new Data.Settings.EnergyBlockParameters();
        }
        private void BuildEnemies()
        {
            EnemyEncounter currentEncounter = GameManager.PersistentGameplayData.CurrentEncounter;

            int numberOfEnemiesToGenerate = Random.Range(currentEncounter.MinEnemiesSpawn, currentEncounter.MaxEnemiesSpawn + 1);
            List<EnemyCharacterData> enemyList = GetRandom.GetRandomItems(currentEncounter.AvailableEnemies, numberOfEnemiesToGenerate);

            for (int i = 0; i < enemyList.Count; i++)
            {
                EnemyBase clone = Instantiate(enemyList[i].EnemyPrefab, EnemyPosList.Count >= i ? EnemyPosList[i] : EnemyPosList[0]);
                clone.BuildCharacter();
                CurrentEnemiesList.Add(clone);
            }
        }
        private void BuildAllies()
        {
            for (var i = 0; i < GameManager.PersistentGameplayData.AllyList.Count; i++)
            {
                AllyBase clone = Instantiate(GameManager.PersistentGameplayData.AllyList[i], AllyPosList.Count >= i ? AllyPosList[i] : AllyPosList[0]);
                clone.BuildCharacter();
                CurrentAlliesList.Add(clone);
            }
        }
        private void TurnStart()
        {
            if (GameManager.PersistentGameplayData.RemainingActiveTurns <= 0)
            {
                CurrentCombatStateType = CombatStateType.EnemyDeclaration;
                return;
            }
                

            if (GameManager.PersistentGameplayData.EnergyBlockRules.Turns > 0)
            {
                GameManager.PersistentGameplayData.EnergyBlockRules.Turns--;
                CurrentCombatStateType = CombatStateType.EnemyDeclaration;
                return;
            }
            EnergyPoolManager.CreateStartOfTurnEnergy();
        }
        private void EnemyActionsDeclaration()
        {
            OnEnemyActionDeclaration?.Invoke();
            CurrentCombatStateType = CombatStateType.AllyTurn;
        }
        private void AllyTurn()
        {
            if (CurrentMainAlly.CharacterStats.IsStunned)
            {
                EndAllyTurn();
                return;
            }
            
            CollectionManager.DrawCards(GameManager.PersistentGameplayData.DrawCount);
            GameManager.PersistentGameplayData.CanSelectCards = true;
        }
        private void EnemyTurn()
        {
            GameManager.PersistentGameplayData.CanSelectCards = false;
            CollectionManager.DiscardHand();
            StartCoroutine(nameof(EnemyTurnRoutine));
        }
        private void TurnEnd()
        {
            OnEnemyStatusTrigger?.Invoke();
            OnAllyStatusTrigger?.Invoke();
            EnergyPoolManager.DecayAllEnergy();
            EnergyPoolManager.OnBlockEnergy?.Invoke();
            if(GameManager.PersistentGameplayData.RemainingActiveTurns-- <= 0 && EnergyPoolManager.CurrentEnergyInPool.Count == 0)
            {
                WinCombat();
            }
            else
            {
                UIManager.CombatCanvas.SetTurnsLeft();
                ScoreManager.TurnsToComplete++;
                CurrentCombatStateType = CombatStateType.TurnStart;
            }
        }
        private void LoseCombat()
        {
            GameManager.PersistentGameplayData.CanSelectCards = false;
            
            CollectionManager.DiscardHand();
            CollectionManager.DiscardPile.Clear();
            CollectionManager.DrawPile.Clear();
            CollectionManager.HandPile.Clear();
            CollectionManager.HandController.hand.Clear();
            UIManager.CombatCanvas.gameObject.SetActive(true);
            UIManager.CombatCanvas.CombatEndPanel.SetActive(true);
        }
        private void WinCombat()
        {
            GameManager.PersistentGameplayData.CanSelectCards = false;
           
            foreach (var allyBase in CurrentAlliesList)
            {
                GameManager.PersistentGameplayData.SetAllyHealthData(allyBase.AllyCharacterData.CharacterID,
                    allyBase.CharacterStats.CurrentHealth, allyBase.CharacterStats.MaxHealth);
            }
            
            CollectionManager.ClearPiles();
            
            CurrentMainAlly.CharacterStats.ClearAllStatus();
            UIManager.CombatCanvas.CombatEndPanel.SetActive(true);
        }
        #endregion
        
        #region Routines
        private IEnumerator EnemyTurnRoutine()
        {
            var waitDelay = new WaitForSeconds(0.1f);

            foreach (var currentEnemy in CurrentEnemiesList)
            {
                yield return currentEnemy.StartCoroutine(nameof(EnemyExample.ActionRoutine));
                yield return waitDelay;
            }

            CurrentCombatStateType = CombatStateType.TurnEnd;
        }
        #endregion
    }
}