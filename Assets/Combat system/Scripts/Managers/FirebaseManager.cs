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

        public void Start()
        {
            
        }
        #endregion
    }
}