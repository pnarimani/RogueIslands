namespace RogueIslands.Gameplay
{
    public interface IDescriptionProvider
    {
        string Get(GameState state, IDescribableItem item);
    }
}