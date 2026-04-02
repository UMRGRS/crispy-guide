using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Characters.Status
{
    public class StatusBase
    {
        private bool decreaseOverTurn = false;
        private bool isPermanent = false;
        private bool isSingleUse = false;
        private bool canNegativeStack = false;

        public bool DecreaseOverTurn { get => decreaseOverTurn; set => decreaseOverTurn = value; }
        public bool IsPermanent { get => isPermanent; set => isPermanent = value; }
        public bool IsSingleUse { get => isSingleUse; set => isSingleUse = value; }
        public bool CanNegativeStack { get => canNegativeStack; set => canNegativeStack = value; }
    }

    
}