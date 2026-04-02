using System;
using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Energy
{
    [Serializable]
    public class EnergyQuantityContainer
    {
        [SerializeField] private EnergyColor color;
        [SerializeField] [Range(1, 10)] private int quantity;    
        public EnergyColor Color => color;
        public int Quantity => quantity;
        public EnergyQuantityContainer(EnergyColor newColor, int newQuantity)
        {
            color = newColor;
            quantity = newQuantity;
        }
    }
}