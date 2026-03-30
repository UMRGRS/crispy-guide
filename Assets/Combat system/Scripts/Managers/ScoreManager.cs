using System.Collections.Generic;
using Firebase.Firestore;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        private ScoreManager(){}
        public static ScoreManager Instance {get; private set;}
        
        #region Private fields
        private int turnsToComplete;
        private int damageDone;
        private int healing;
        private int usedCards;
        private int usedEnergy;
        
        #endregion

        #region Public getters/setters
        public int TurnsToComplete { get => turnsToComplete; set => turnsToComplete = value; }
        public int DamageDone { get => damageDone; set => damageDone = value; }
        public int Healing { get => healing; set => healing = value; }
        public int UsedCards { get => usedCards; set => usedCards = value; }
        public int UsedEnergy { get => usedEnergy; set => usedEnergy = value; }
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
        public Dictionary<string, object> GetScoreObject(string playerName)
        {
            return new Dictionary<string, object>()
            {
                { "name", $"{playerName}" },
                { "total", CalculateScore() },
                { "cards", UsedCards },
                { "damage", DamageDone },
                { "energy", UsedEnergy },
                { "heal", Healing },
                { "turns", TurnsToComplete },
                { "date", FieldValue.ServerTimestamp },
            }; 
        }

        public void ClearScore()
        {
            turnsToComplete = 0;
            damageDone = 0;
            healing = 0;
            usedCards = 0;
            usedEnergy = 0;
        }
        #endregion
        
        #region  Private methods
        private int CalculateScore()
        {
            int total = 0;
            
            total += Mathf.Max(0, 100 - 10 * TurnsToComplete);

            total += 50 * (DamageDone / 10);

            total += 50 * (Healing / 5);

            total += Mathf.Max(0, 200 - 10 * UsedCards);

            total += Mathf.Max(0, 300 - 10 * UsedEnergy);

            return total;
        }
        #endregion
    }
}