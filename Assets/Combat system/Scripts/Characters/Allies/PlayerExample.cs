using NueGames.NueDeck.Scripts.Managers;

namespace NueGames.NueDeck.Scripts.Characters.Allies
{
    public class PlayerExample : AllyBase
    {
        public override void BuildCharacter()
        {
            base.BuildCharacter();
            CharacterStats.SetCurrentHealth(CharacterStats.CurrentHealth);
        }
    }
}