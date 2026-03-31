using System;
using System.Collections;
using NueGames.NueDeck.Scripts.Managers;
using NueGames.NueDeck.ThirdParty.NueTooltip.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using NueGames.NueDeck.Scripts.Enums;


namespace NueGames.NueDeck.Scripts.Utils
{
    public class SceneChanger : MonoBehaviour
    {
        private GameManager GameManager => GameManager.Instance;
        private UIManager UIManager => UIManager.Instance;
        private AudioManager AudioManager => AudioManager.Instance;
        
        private enum SceneType
        {
            MainMenu,
            Map,
            Combat
        }
        public void OpenMainMenuScene()
        {
            StartCoroutine(DelaySceneChange(SceneType.MainMenu, AudioActionType.MenuMusic));
        }
        public void OpenMapScene()
        {
            StartCoroutine(DelaySceneChange(SceneType.Map, AudioActionType.MenuMusic));
        }
        public void OpenCombatSceneWithFloor(int floorId)
        {
            GameManager.PersistentGameplayData.CurrentFloor = (FloorId)floorId;
            StartCoroutine(DelaySceneChange(SceneType.Combat, AudioActionType.BattleMusic));
        }
        public void ExitApp()
        {
            GameManager.OnExitApp();
            Application.Quit();
        }
        private IEnumerator DelaySceneChange(SceneType type, AudioActionType musicType)
        {
            UIManager.SetCanvas(UIManager.Instance.InventoryCanvas,false,true);
            yield return StartCoroutine(UIManager.Instance.Fade(true));

            switch (type)
            {
                case SceneType.MainMenu:
                    UIManager.ChangeScene(GameManager.SceneData.mainMenuSceneIndex);
                    UIManager.SetCanvas(UIManager.CombatCanvas,false,true);
                    UIManager.SetCanvas(UIManager.RewardCanvas,false,true);
                   
                    GameManager.InitGameplayData();
                    GameManager.SetInitialHand();
                    break;
                case SceneType.Map:
                    UIManager.ChangeScene(GameManager.SceneData.mapSceneIndex);
                    UIManager.SetCanvas(UIManager.CombatCanvas,false,true);
                    UIManager.SetCanvas(UIManager.RewardCanvas,false,true);
                   
                    break;
                case SceneType.Combat:
                    UIManager.ChangeScene(GameManager.SceneData.combatSceneIndex);
                    UIManager.SetCanvas(UIManager.CombatCanvas,false,true);
                    UIManager.SetCanvas(UIManager.RewardCanvas,false,true);
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            AudioManager.PlayMusic(musicType);
        }
    }
}
