namespace NueGames.NueDeck.Scripts.Managers
{
    public class ManagersContainer
    {
        protected FxManager FxManager => FxManager.Instance;
        protected AudioManager AudioManager => AudioManager.Instance;
        protected GameManager GameManager => GameManager.Instance;
        protected CombatManager CombatManager => CombatManager.Instance;
        protected CollectionManager CollectionManager => CollectionManager.Instance;
        protected EnergyPoolManager EnergyPoolManager => EnergyPoolManager.Instance;
    }
}