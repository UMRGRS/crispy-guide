using System;
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Data.Containers;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Utils.Background
{
    public class BackgroundContainer : MonoBehaviour
    {
        [SerializeField] private List<BackgroundRoot> backgroundRootList;
        public List<BackgroundRoot> BackgroundRootList => backgroundRootList;
        
        private GameManager GameManager => GameManager.Instance;
        
        public void OpenSelectedBackground()
        {
            EnemyEncounter encounter = GameManager.PersistentGameplayData.CurrentEncounter;
            foreach (BackgroundRoot backgroundRoot in BackgroundRootList)
                backgroundRoot.gameObject.SetActive(encounter.TargetBackgroundType == backgroundRoot.BackgroundType);
        }
    }
}