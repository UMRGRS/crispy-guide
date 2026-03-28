using Firebase;
using Firebase.Extensions;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.FirebaseImplementation
{
public class FirebaseInitializer : MonoBehaviour
{
    public void Start()
    {
        Debug.Log("Started");
        FirebaseApp.CheckAndFixDependenciesAsync()
            .ContinueWithOnMainThread(task =>
        {
            var status = task.Result;

            if (status == DependencyStatus.Available)
            {
                Debug.Log("Firebase Ready");
                InitializeFirestore();
            }
            else
            {
                Debug.LogError($"Firebase Error: {status}");
            }
        });
    }

    void InitializeFirestore()
    {
        var db = Firebase.Firestore.FirebaseFirestore.DefaultInstance;
        Debug.Log("Firestore initialized");
    }
}
}