using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "New cost data", menuName = "NueDeck/Collection/Energy cost",order = 0)]
    public class EnergyQuantityData : ScriptableObject
    {
        [SerializeField] private EnergyColor color;
        [Range(1, 10)] [SerializeField] private int quantity;
    
        public EnergyColor Color => color;
        public int Quantity => quantity;
        public void Initialize(EnergyColor newColor, int newQuantity)
        {
            color = newColor;
            quantity = newQuantity;
        }
    }
}