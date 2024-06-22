using System;
using System.Linq;
using RogueIslands.Boosters.Actions;
using UnityEngine;

namespace RogueIslands.Boosters.Executors
{
    public abstract class GameActionExecutor
    {
        public abstract Type ActionType { get; }
        public abstract void Execute(GameState state, IGameView view, IBooster booster, GameAction action);
    }
    
    public abstract class GameActionExecutor<T> : GameActionExecutor 
        where T : GameAction
    {
        public override Type ActionType { get; } = typeof(T);

        public sealed override void Execute(GameState state, IGameView view, IBooster booster, GameAction action)
        {
            try
            {
                if (action.Conditions != null && action.Conditions.Any(condition => !state.IsConditionMet(booster, condition)))
                    return;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to evaluate conditions for action `{action.GetType().Name}` on booster {booster.Id}: {e.Message}");
                return;
            }

            var boosterView = view.GetBooster(booster);
            
            boosterView.OnBeforeActionExecuted(state, action);
            Execute(state, view, booster, (T)action);
            boosterView.OnAfterActionExecuted(state, action);
        }

        protected abstract void Execute(GameState state, IGameView view, IBooster booster, T action);
    }
}