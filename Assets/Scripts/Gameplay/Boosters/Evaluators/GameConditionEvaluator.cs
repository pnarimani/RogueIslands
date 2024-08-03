using System;
using Autofac.Features.ResolveAnything;
using RogueIslands.Diagnostics;
using RogueIslands.Gameplay.Boosters.Conditions;
using UnityEngine;

namespace RogueIslands.Gameplay.Boosters.Evaluators
{
    public abstract class GameConditionEvaluator
    {
        public abstract bool CanHandle(Type conditionType);
        public abstract bool Evaluate(GameState state, IBooster booster, IGameCondition condition);
    }

    public abstract class GameConditionEvaluator<T> : GameConditionEvaluator where T : class, IGameCondition
    {
        public override bool CanHandle(Type conditionType) => conditionType == typeof(T);

        public sealed override bool Evaluate(GameState state, IBooster booster, IGameCondition condition)
        {
            using var profiler = new ProfilerScope("GameConditionEvaluator.Evaluate");
            using var conditionProfiler = new ProfilerScope(typeof(T).Name);

            if (condition is not T gameCondition)
            {
                Debug.LogError($"Invalid condition type: {condition.GetType().Name}. Expected: {typeof(T).Name}");
                return false;
            }
            
            return Evaluate(state, booster, gameCondition);
        }

        protected abstract bool Evaluate(GameState state, IBooster booster, T condition);
    }
}