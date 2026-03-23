using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Data.Energy;
using TMPro;
using Unity.VisualScripting;
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
        [SerializeField] private Image noCostIntentImage;
        [SerializeField] private TextMeshProUGUI redCostValue;
        [SerializeField] private TextMeshProUGUI blueCostValue;
        [SerializeField] private TextMeshProUGUI greenCostValue;

        public void SetEnemyIntention(EnemyAbilityData ability)
        {
            ActionCostData totalEnergyCost = ability.Card.GatherCardCosts();

            if(totalEnergyCost.RedCost == 0 && totalEnergyCost.BlueCost == 0 && totalEnergyCost.GreenCost == 0)
            {
                noCostIntentImage.gameObject.SetActive(true);
                return;
            }

            SetValueVisibility(!ability.HideActionValue);

            redCostValue.text = totalEnergyCost.RedCost.ToString();
            redIntentImage.gameObject.SetActive(totalEnergyCost.RedCost > 0);

            blueCostValue.text = totalEnergyCost.BlueCost.ToString();
            blueIntentImage.gameObject.SetActive(totalEnergyCost.BlueCost > 0);

            greenCostValue.text = totalEnergyCost.GreenCost.ToString();
            greenIntentImage.gameObject.SetActive(totalEnergyCost.GreenCost > 0);
        }

        public void SetIntentionVisibility(bool value)
        {
            redIntentImage.gameObject.SetActive(value);
            blueIntentImage.gameObject.SetActive(value);
            greenIntentImage.gameObject.SetActive(value);
            noCostIntentImage.gameObject.SetActive(value);
        }

        public void SetValueVisibility(bool value)
        {
            redCostValue.gameObject.SetActive(value);
            blueCostValue.gameObject.SetActive(value);
            greenCostValue.gameObject.SetActive(value);
        }
    
    }
}