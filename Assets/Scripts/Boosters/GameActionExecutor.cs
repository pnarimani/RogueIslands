using System;
using System.Linq;

namespace RogueIslands.Boosters
{
    public abstract class GameActionExecutor
    {
        public abstract Type ActionType { get; }
        public abstract void Execute(GameState state, GameAction action);
    }
    
    public abstract class GameActionExecutor<T> : GameActionExecutor where T : GameAction
    {
        public override Type ActionType { get; } = typeof(T);

        public sealed override void Execute(GameState state, GameAction action)
        {
            if (action.Conditions.Any(condition => !state.IsConditionMet(condition)))
                return;

            Execute(state, (T)action);
        }

        protected abstract void Execute(GameState state, T action);
    }
}