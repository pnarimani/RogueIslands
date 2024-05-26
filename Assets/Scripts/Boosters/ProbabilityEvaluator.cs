using System;

namespace RogueIslands.Boosters
{
    public sealed class ProbabilityEvaluator : ConditionEvaluator<ProbabilityCondition>
    {
        private readonly Random _random;

        public ProbabilityEvaluator(Random random)
        {
            _random = random;
        }
        
        protected override bool Evaluate(GameState state,ProbabilityCondition condition)
        {
            return _random.Next(0, condition.TotalOutcomes) < condition.FavorableOutcome;
        }
    }
}