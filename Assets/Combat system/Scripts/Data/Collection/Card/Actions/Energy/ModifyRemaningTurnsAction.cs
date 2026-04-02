using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "New modify remaining turns action", menuName = "NueDeck/Collection/Actions/Energy/Modify remaining turns",order = 0)]
    public class ModifyRemainingTurnsAction : CardActionData
    {
        [Header("BModify remaining turns settings")]
        [SerializeField] private RemainingTurnsModificationType type;
        [Range(1,10)] [SerializeField] private int value;

        public RemainingTurnsModificationType Type => type;
        public int Value => value;
        
        public override bool CanExecute(CardExecutionContext context)
        {
            if(!context.managersContainer.EnergyPoolManager.IsEnergyOnPool(GetTotalCost())) return false;

            return true; 
        }
        public override void Execute(CardExecutionContext context)
        {
            PayCost(context);
            
            context.managersContainer.GameManager.ModifyRemainingTurns(value, type);
            context.managersContainer.UIManager.CombatCanvas.SetTurnsLeft();
            // Add FX effects

            // Add audio effects
        }
        public override string GetActionDescription(CardExecutionContext context)
        {

            var modType = type.Equals(RemainingTurnsModificationType.Increase) ? "Increase" : "Decrease";
            
            return BuildActionDescription($"{modType} the remaining energy generation turns by {value}");
        }
    }
}