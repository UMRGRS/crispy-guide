using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    [CreateAssetMenu(fileName = "New cost data", menuName = "NueDeck/Collection/Energy cost",order = 0)]
    public class EnergyQuantityData : ScriptableObject
    {
        [SerializeField] private EnergyColor energyColor;
        [Range(1, 10)] [SerializeField] private int quantity;
    
        public EnergyColor EnergyColor => energyColor;
        public int Quantity => quantity;
        public EnergyQuantityData(EnergyColor newColor, int newQuantity)
        {
            energyColor = newColor;
            quantity = newQuantity;
        }
    }
}