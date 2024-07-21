using System;
using RogueIslands.Gameplay.Boosters.Conditions;
using RogueIslands.Gameplay.Rand;

namespace RogueIslands.Gameplay.Boosters.Evaluators
{
    public sealed class ProbabilityEvaluator : GameConditionEvaluator<ProbabilityCondition>
    {
        private readonly RogueRandom _random = new((uint)Environment.TickCount); // TODO: Think about this

        protected override bool Evaluate(GameState state, IBooster booster, ProbabilityCondition condition)
        {
            var roll = _random.ForAct(state.Act).NextInt(0, condition.TotalOutcomes);
            return roll < condition.FavorableOutcome * (state.GetRiggedCount() + 1);
        }
    }
}