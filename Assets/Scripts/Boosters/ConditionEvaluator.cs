using System;

namespace RogueIslands.Boosters
{
    public abstract class ConditionEvaluator
    {
        public abstract Type ConditionType { get; }
        public abstract bool Evaluate(GameState state, IGameCondition condition);
    }

    public abstract class ConditionEvaluator<T> : ConditionEvaluator where T : IGameCondition
    {
        public override Type ConditionType { get; } = typeof(T);

        public sealed override bool Evaluate(GameState state, IGameCondition condition)
        {
            return Evaluate(state, (T)condition);
        }

        protected abstract bool Evaluate(GameState state, T condition);
    }
}