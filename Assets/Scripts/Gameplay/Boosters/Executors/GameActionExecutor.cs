using System;
using RogueIslands.Diagnostics;
using RogueIslands.Gameplay.Boosters.Actions;

namespace RogueIslands.Gameplay.Boosters.Executors
{
    public abstract class GameActionExecutor
    {
        public abstract Type ActionType { get; }
        public abstract bool Execute(GameState state, IGameView view, IBooster booster, GameAction action);
    }
    
    public abstract class GameActionExecutor<T> : GameActionExecutor 
        where T : GameAction
    {
        public override Type ActionType { get; } = typeof(T);

        public sealed override bool Execute(GameState state, IGameView view, IBooster booster, GameAction action)
        {
            using var profiler = new ProfilerScope("GameActionExecutor.Execute");
            using var conditionProfiler = new ProfilerScope(typeof(T).Name);
            
            Execute(state, view, booster, (T)action);
            return true;
        }

        protected abstract void Execute(GameState state, IGameView view, IBooster booster, T action);
    }
}