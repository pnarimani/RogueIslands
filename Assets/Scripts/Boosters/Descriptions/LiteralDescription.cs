namespace RogueIslands.Boosters.Descriptions
{
    public class LiteralDescription : IDescriptionProvider
    {
        public LiteralDescription(string text) => Text = text;

        public string Text { get; }

        public string Get(IDescribableItem item) => Text;
    }
}