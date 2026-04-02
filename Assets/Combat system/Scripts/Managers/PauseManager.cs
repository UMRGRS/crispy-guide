using UnityEngine;

namespace NueGames.NueDeck.Scripts.Managers
{
    public class PauseManager : MonoBehaviour
    {
        public static bool IsPaused { get; private set; }

        public static void SetPause(bool pause)
        {
            IsPaused = pause;

            Time.timeScale = pause ? 0f : 1f;
        }

        public static void TogglePause()
        {
            SetPause(!IsPaused);
        }
    }
}