namespace RogueIslands.Gameplay.DeckBuilding
{
    public abstract class DeckAction
    {
        public abstract void Execute(GameState state, IGameView view);
    }
}