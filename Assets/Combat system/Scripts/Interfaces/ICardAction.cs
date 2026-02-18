using NueGames.NueDeck.Scripts.Enums;

public interface ICardAction
{
    CardActionType ActionType { get; }
    void DoAction(object parameters);
}
