using Unity.Mathematics;

namespace RogueIslands.Boosters
{
    public sealed class ProbabilityEvaluator : GameConditionEvaluator<ProbabilityCondition>
    {
        public int FavorableOutcomeModification { get; set; }
        private Random _random;

        public ProbabilityEvaluator(Random random)
        {
            _random = random;
        }
        
        protected override bool Evaluate(GameState state, IBooster booster, ProbabilityCondition condition)
        {
            return _random.NextInt(0, condition.TotalOutcomes) < condition.FavorableOutcome + FavorableOutcomeModification;
        }
    }
}