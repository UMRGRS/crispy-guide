using System.Collections;
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Energy;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Managers
{
    public class EnergyPoolManager : MonoBehaviour
    {
        private EnergyPoolManager(){}
        public static EnergyPoolManager Instance {get; private set;}

        [Header("References")]
        [SerializeField] private List<Transform> energyPosList;

        #region Cache
        public List<EnergyBase> CurrentEnergyInPool {get; private set;} = new List<EnergyBase>();
        public List<Transform> EnergyPostList => energyPosList;
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
            }
        }
        #endregion
    }
}

