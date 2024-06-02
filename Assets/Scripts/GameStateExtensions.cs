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

        public static void Execute(this GameState state, Booster booster, GameAction action)
        {
            _defaultExecutors ??= LifetimeScopeProvider.Get().Container.Resolve<IReadOnlyList<GameActionExecutor>>();
            _defaultExecutors.First(x => x.ActionType == action.GetType()).Execute(state, booster, action);
        }

        public static bool IsConditionMet(this GameState state, IGameCondition condition)
        {
            _defaultEvaluators ??= LifetimeScopeProvider.Get().Container.Resolve<IReadOnlyList<ConditionEvaluator>>();

            var evaluator = _evaluatorOverrides.FirstOrDefault(x => x.ConditionType == condition.GetType()) ??
                            _defaultEvaluators.First(x => x.ConditionType == condition.GetType());

            return evaluator.Evaluate(state, condition);
        }

        public static void AddBooster(this GameState state, Booster booster)
        {
            state.Boosters.Add(booster);
            state.Execute(booster, booster.BuyAction);

            if (booster.EvaluationOverrides != null)
                _evaluatorOverrides.AddRange(booster.EvaluationOverrides);

            state.CurrentEvent = "BoosterBought";
            state.ExecuteAll();
        }

        public static void RemoveBooster(this GameState state, Booster booster)
        {
            state.Boosters.Remove(booster);
            state.Execute(booster, booster.SellAction);

            if (booster.EvaluationOverrides != null)
                _evaluatorOverrides.RemoveAll(booster.EvaluationOverrides.Contains);

            state.CurrentEvent = "BoosterSold";
            state.ExecuteAll();
        }

        public static void ExecuteAll(this GameState state)
        {
            foreach (var booster in state.Boosters)
                state.Execute(booster, booster.EventAction);
        }
    }
}