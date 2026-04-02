using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using NueGames.NueDeck.Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NueGames.NueDeck.Scripts.UI
{
    public class CombatCanvas : CanvasBase
    {
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI turnsLeftTextField;
        [SerializeField] private List<Button> buttonsToDisable;
        [SerializeField] private SceneChanger sceneChanger;

        [Header("Panels")]
        [SerializeField] private GameObject combatEndPanel;
        [SerializeField] private GameObject combatPausePanel;

        public TextMeshProUGUI TurnsLeftTextField => turnsLeftTextField;
        public GameObject CombatEndPanel => combatEndPanel;
        public GameObject CombatPausePanel => combatPausePanel;



        #region Setup
        private void Awake()
        {
            combatEndPanel.SetActive(false);
            combatPausePanel.SetActive(false);
        }   
        #endregion

        #region Public Methods
        public void SetTurnsLeft()
        {
            turnsLeftTextField.text = $"Turns left: {GameManager.PersistentGameplayData.RemainingActiveTurns}";
        }

        public override void ResetCanvas()
        {
            base.ResetCanvas();
            TogglePauseButtons();
            combatEndPanel.SetActive(false);
            combatPausePanel.SetActive(false);
        }

        public void EndTurn()
        {
            if (CombatManager.CurrentCombatStateType == CombatStateType.AllyTurn)
                CombatManager.EndAllyTurn();
        }

        public void ToCombatSelect()
        {
            PauseManager.TogglePause();
            sceneChanger.OpenMapScene();
        }

        public void TogglePause()
        {
            PauseManager.TogglePause();
            combatPausePanel.SetActive(PauseManager.IsPaused);
            TogglePauseButtons();
        }

        private void TogglePauseButtons()
        {
            foreach(var button in buttonsToDisable)
            {
                button.interactable = !PauseManager.IsPaused;
            }
        }
        #endregion
    }
}