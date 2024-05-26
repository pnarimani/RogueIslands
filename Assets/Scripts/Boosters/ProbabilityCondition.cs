namespace RogueIslands.Boosters
{
    public class ProbabilityCondition : IGameCondition
    {
        public int FavorableOutcome { get; set; }
        public int TotalOutcomes { get; set; }
    }
}