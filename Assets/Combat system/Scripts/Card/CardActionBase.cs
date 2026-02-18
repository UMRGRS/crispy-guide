using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;

namespace NueGames.NueDeck.Scripts.Card
{
    public class CardActionParameters
    {
        public readonly float Value;
        public readonly CharacterBase TargetCharacter;
        public readonly CharacterBase SelfCharacter;
        public readonly CardData CardData;
        public readonly CardBase CardBase;
        public CardActionParameters(float value,CharacterBase target, CharacterBase self,CardData cardData, CardBase cardBase)
        {
            Value = value;
            TargetCharacter = target;
            SelfCharacter = self;
            CardData = cardData;
            CardBase = cardBase;
        }
    }

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

    public abstract class CardActionBase<TParameters> : ICardAction
    {
        protected CardActionBase(){}
        public abstract CardActionType ActionType { get;}
        public abstract void DoAction(TParameters parameters);
        public void DoAction(object parameters)
        {
            DoAction((TParameters)parameters);
        }

        protected FxManager FxManager => FxManager.Instance;
        protected AudioManager AudioManager => AudioManager.Instance;
        protected GameManager GameManager => GameManager.Instance;
        protected CombatManager CombatManager => CombatManager.Instance;
        protected CollectionManager CollectionManager => CollectionManager.Instance;
        protected EnergyPoolManager EnergyPoolManager => EnergyPoolManager.Instance;
    }
    
    
   
}