using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Managers;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    public class CardExecutionContext
    {
        public CharacterBase source;
        public CharacterBase target;
        public bool registerScore;
        public ManagersContainer managersContainer;
        public CardExecutionContext(CharacterBase source = null, CharacterBase target = null, bool registerScore = false)
        {
            this.source = source;
            this.target = target;
            this.registerScore = registerScore;
            managersContainer = new ManagersContainer();
        }
    }
}