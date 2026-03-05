using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;

namespace NueGames.NueDeck.Scripts.Card
{
    public class CardEnergyActionParameters
    {
        public readonly List<EnergyQuantityData> EnergyCreationList;
        public readonly List<EnergyConversion> EnergyConversionList;
        public readonly List<EnergyStrengthModification> EnergyStrengthModificationList;

        public CardEnergyActionParameters(List<EnergyQuantityData> energyCreationList, List<EnergyConversion> energyConversionList, List<EnergyStrengthModification> energyStrengthModificationList)
        {
            EnergyCreationList = energyCreationList;
            EnergyConversionList = energyConversionList;
            EnergyStrengthModificationList = energyStrengthModificationList;
        }
    }

    public abstract class EnergyCardActionBase : ManagersContainer
    {
        public abstract EnergyCardActionType ActionType { get; }
        public abstract void DoAction(CardEnergyActionParameters parameters);
    }
}