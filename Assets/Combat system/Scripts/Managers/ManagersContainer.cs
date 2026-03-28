namespace NueGames.NueDeck.Scripts.Managers
{
    public class ManagersContainer
    {
        public FxManager FxManager => FxManager.Instance;
        public AudioManager AudioManager => AudioManager.Instance;
        public GameManager GameManager => GameManager.Instance;
        public CombatManager CombatManager => CombatManager.Instance;
        public CollectionManager CollectionManager => CollectionManager.Instance;
        public EnergyPoolManager EnergyPoolManager => EnergyPoolManager.Instance;
        public ScoreManager ScoreManager => ScoreManager.Instance;
    }
}