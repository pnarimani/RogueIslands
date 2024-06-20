namespace RogueIslands.Boosters.Descriptions
{
    public class LiteralDescription : IDescriptionProvider
    {
        private readonly string _literalText;

        public LiteralDescription(string literalText) => _literalText = literalText;

        public string Get(BoosterCard booster) => _literalText;
    }
}