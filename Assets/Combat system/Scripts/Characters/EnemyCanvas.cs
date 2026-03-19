using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Data.Collection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NueGames.NueDeck.Scripts.Characters
{
    public class EnemyCanvas : CharacterCanvas
    {
        [Header("Enemy Canvas Settings")]
        [SerializeField] private Image redIntentImage;
        [SerializeField] private Image blueIntentImage;
        [SerializeField] private Image greenIntentImage;
        [SerializeField] private TextMeshProUGUI redCostValue;
        [SerializeField] private TextMeshProUGUI blueCostValue;
        [SerializeField] private TextMeshProUGUI greenCostValue;

        public void SetEnemyIntention(EnemyAbilityData ability)
        {
            
        }

        public void ClearIntention()
        {
            redIntentImage.gameObject.SetActive(false);
            blueIntentImage.gameObject.SetActive(false);
            greenIntentImage.gameObject.SetActive(false);
        }
    
    }
}