using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Containers
{
    [CreateAssetMenu(fileName = "Enemy Intention", menuName = "NueDeck/Containers/EnemyIntention", order = 0)]
    public class EnemyIntentionData : ScriptableObject
    {
        [SerializeField] private EnemyTargetType targetType;
        [SerializeField] private Sprite intentionSprite;

        public EnemyTargetType TargetType => targetType;
        public Sprite IntentionSprite => intentionSprite;
    }
}