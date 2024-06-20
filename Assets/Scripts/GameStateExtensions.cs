using System;
using System.Collections.Generic;
using System.Linq;
using RogueIslands.Boosters;
using UnityEngine.Assertions;

namespace RogueIslands
{
    public static class GameStateExtensions
    {
        private static IReadOnlyList<ConditionEvaluator> _defaultEvaluators;
        private static IReadOnlyList<GameActionExecutor> _defaultExecutors;
        private static readonly List<ConditionEvaluator> _evaluatorOverrides = new();

        public static void Execute(this GameState state, IGameView view, BoosterCard booster, GameAction action)
        {
            Assert.IsNotNull(action);
            Assert.IsNotNull(booster);
            Assert.IsNotNull(view);
            
            _defaultExecutors ??= StaticResolver.Resolve<IReadOnlyList<GameActionExecutor>>();

            Assert.IsNotNull(_defaultExecutors);
            
            var exec = _defaultExecutors.FirstOrDefault(x => x.ActionType == action.GetType());
            if(exec == null)
                throw new InvalidOperationException($"No executor found for action type {action.GetType().Name}");
            exec.Execute(state, view, booster, action);
        }

        public static bool IsConditionMet(this GameState state, IGameCondition condition)
        {
            Assert.IsNotNull(condition);
            
            _defaultEvaluators ??= StaticResolver.Resolve<IReadOnlyList<ConditionEvaluator>>();
            
            Assert.IsNotNull(_evaluatorOverrides);
            Assert.IsNotNull(_defaultEvaluators);

            var evaluator = _evaluatorOverrides.FirstOrDefault(x => x.ConditionType == condition.GetType()) ??
                            _defaultEvaluators.First(x => x.ConditionType == condition.GetType());

            Assert.IsNotNull(evaluator);
            
            return evaluator.Evaluate(state, condition);
        }

        public static void ExecuteEvent(this GameState state, IGameView view, string eventName)
        {
            state.CurrentEvent = eventName;
            
            foreach (var booster in state.Boosters)
            {
                if (booster.EventAction != null)
                    state.Execute(view, booster, booster.EventAction);
            }
        }

        public static void AddBooster(this GameState state, IGameView view, BoosterCard booster)
        {
            var instance = booster.Clone();
            instance.Id = new BoosterInstanceId(Guid.NewGuid().GetHashCode());
            
            state.Boosters.Add(instance);
            view.AddBooster(instance);
            
            if (instance.BuyAction != null) 
                state.Execute(view, instance, instance.BuyAction);

            if (instance.EvaluationOverrides != null)
                _evaluatorOverrides.AddRange(instance.EvaluationOverrides);
            
            state.ExecuteEvent(view, "BoosterBought");
        }

        public static void RemoveBooster(this GameState state, IGameView view, BoosterInstanceId boosterId)
        {
            var booster = state.Boosters.First(x => x.Id == boosterId);
            
            state.Boosters.Remove(booster);
            if (booster.SellAction != null) 
                state.Execute(view, booster, booster.SellAction);

            if (booster.EvaluationOverrides != null)
                _evaluatorOverrides.RemoveAll(booster.EvaluationOverrides.Contains);

            view.RemoveBooster(booster);
            state.Money += booster.SellPrice;
            view.GetUI().RefreshAll();

            state.ExecuteEvent(view, "BoosterSold");
            state.ExecuteEvent(view, "BoosterRemoved");
        }
        
        public static void DestroyBooster(this GameState state, IGameView view, BoosterInstanceId boosterId)
        {
            var booster = state.Boosters.First(x => x.Id == boosterId);
            
            state.Boosters.Remove(booster);

            if (booster.EvaluationOverrides != null)
                _evaluatorOverrides.RemoveAll(booster.EvaluationOverrides.Contains);

            view.RemoveBooster(booster);
            view.GetUI().RefreshAll();

            state.ExecuteEvent(view, "BoosterRemoved");
        }
    }
}