namespace RogueIslands.Gameplay.Boosters.Conditions
{
    public class ProbabilityCondition : IGameCondition
    {
        public int FavorableOutcome { get; set; }
        public int TotalOutcomes { get; set; }

        public ProbabilityCondition()
        {
        }

        public ProbabilityCondition(int favorableOutcome, int totalOutcomes)
        {
            FavorableOutcome = favorableOutcome;
            TotalOutcomes = totalOutcomes;
        }
    }
}