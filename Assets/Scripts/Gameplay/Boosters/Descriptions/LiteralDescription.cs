namespace RogueIslands.Gameplay.Boosters.Descriptions
{
    public class LiteralDescription : IDescriptionProvider
    {
        public LiteralDescription(string text) => Text = text;

        public string Text { get; }

        public string Get(GameState state, IDescribableItem item) => Text;
    }
}