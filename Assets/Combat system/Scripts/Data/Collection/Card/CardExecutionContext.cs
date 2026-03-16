using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Managers;

namespace NueGames.NueDeck.Scripts.Data.Collection
{
    public class CardExecutionContext
    {
        public CharacterBase source;
        public CharacterBase target;
        public ManagersContainer managersContainer;
        public CardExecutionContext(CharacterBase source = null, CharacterBase target = null)
        {
            this.source = source;
            this.target = target;
            managersContainer = new ManagersContainer();
        }
    }
}