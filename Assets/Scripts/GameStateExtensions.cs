using System;
using System.Collections.Generic;
using System.Linq;
using RogueIslands.Boosters;
using VContainer;

namespace RogueIslands
{
    public static class GameStateExtensions
    {
        private static IReadOnlyList<ConditionEvaluator> _defaultEvaluators;
        private static IReadOnlyList<GameActionExecutor> _defaultExecutors;
        private static readonly List<ConditionEvaluator> _evaluatorOverrides = new();

        public static void Execute(this GameState state, IGameView view, Booster booster, GameAction action)
        {
            _defaultExecutors ??= LifetimeScopeProvider.Get().Container.Resolve<IReadOnlyList<GameActionExecutor>>();
            _defaultExecutors.First(x => x.ActionType == action.GetType()).Execute(state, view, booster, action);
        }

        public static bool IsConditionMet(this GameState state, IGameCondition condition)
        {
            _defaultEvaluators ??= LifetimeScopeProvider.Get().Container.Resolve<IReadOnlyList<ConditionEvaluator>>();

            var evaluator = _evaluatorOverrides.FirstOrDefault(x => x.ConditionType == condition.GetType()) ??
                            _defaultEvaluators.First(x => x.ConditionType == condition.GetType());

            return evaluator.Evaluate(state, condition);
        }

        public static void ExecuteAll(this GameState state, IGameView view)
        {
            foreach (var booster in state.Boosters)
                state.Execute(view, booster, booster.EventAction);
        }

        public static void AddBooster(this GameState state, IGameView view, Booster booster)
        {
            var instance = booster.Clone();
            instance.Id = new BoosterInstanceId(Guid.NewGuid().GetHashCode());
            
            state.Boosters.Add(instance);
            state.Execute(view, instance, instance.BuyAction);

            if (instance.EvaluationOverrides != null)
                _evaluatorOverrides.AddRange(instance.EvaluationOverrides);

            state.CurrentEvent = "BoosterBought";
            state.ExecuteAll(view);
        }

        public static void RemoveBooster(this GameState state, IGameView view, BoosterInstanceId boosterId)
        {
            var booster = state.Boosters.First(x => x.Id == boosterId);
            
            state.Boosters.Remove(booster);
            state.Execute(view, booster, booster.SellAction);

            if (booster.EvaluationOverrides != null)
                _evaluatorOverrides.RemoveAll(booster.EvaluationOverrides.Contains);

            state.CurrentEvent = "BoosterSold";
            state.ExecuteAll(view);
        }
    }
}