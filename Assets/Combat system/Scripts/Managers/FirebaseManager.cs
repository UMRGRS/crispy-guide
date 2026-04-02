using UnityEngine;
using Firebase.Firestore;
using System.Collections.Generic;

namespace NueGames.NueDeck.Scripts.Managers
{
    public class FirebaseManager : MonoBehaviour
    {
        private FirebaseManager(){}
        public static FirebaseManager Instance {get; private set;}

        #region private fields
        private FirebaseFirestore db;
        private readonly string scoreCollectionName = "scores";
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
                db = FirebaseFirestore.DefaultInstance;
            }
        }
        #endregion

        #region Public methods
        
        public void SaveScore(Dictionary<string, object> data)
        {
            db.Collection(scoreCollectionName).Document().SetAsync(data);
        }

        #endregion
    }
}