using System;
using System.Collections;
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Data.Energy;
using NueGames.NueDeck.Scripts.Energy;
using NueGames.NueDeck.Scripts.Enums;
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

        #region Public methods
        //return a list of the specified energies if they exist on the pool
        //ej. if a card lists (red, blue) returns (red, blue) otherwise an empty array
        public List<EnergyBase> FindEnergy(Dictionary<int, EnergyColor> energies)
        {
            throw new NotImplementedException();
        }
        //create new energy based on the passed data and add it to the pool
        public void CreateEnergy(List<EnergyData> energies)
        {
            //call build energy to get the required object
            throw new NotImplementedException();
        }
        public void ConvertColor()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

