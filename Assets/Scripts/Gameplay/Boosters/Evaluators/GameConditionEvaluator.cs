using System;
using RogueIslands.Gameplay.Boosters.Conditions;

namespace RogueIslands.Gameplay.Boosters.Evaluators
{
    public abstract class GameConditionEvaluator
    {
        public abstract Type ConditionType { get; }
        public abstract bool Evaluate(GameState state, IBooster booster, IGameCondition condition);
    }

    public abstract class GameConditionEvaluator<T> : GameConditionEvaluator where T : IGameCondition
    {
        public override Type ConditionType { get; } = typeof(T);

        public sealed override bool Evaluate(GameState state, IBooster booster, IGameCondition condition)
        {
            return Evaluate(state, booster, (T)condition);
        }

        protected abstract bool Evaluate(GameState state, IBooster booster, T condition);
    }
}