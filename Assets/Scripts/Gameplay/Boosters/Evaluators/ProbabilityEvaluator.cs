using System;
using RogueIslands.Gameplay.Boosters.Conditions;
using RogueIslands.Gameplay.Rand;

namespace RogueIslands.Gameplay.Boosters.Evaluators
{
    public sealed class ProbabilityEvaluator : GameConditionEvaluator<ProbabilityCondition>
    {
        protected override bool Evaluate(GameState state, IBooster booster, ProbabilityCondition condition)
        {
            var rand = state.GetRandomForType<ProbabilityCondition>().ForAct(state.Act);
            var roll = rand.NextInt(0, condition.TotalOutcomes);
            return roll < condition.FavorableOutcome * (state.GetRiggedCount() + 1);
        }
    }
}