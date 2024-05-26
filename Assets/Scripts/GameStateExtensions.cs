using System;
using System.Collections.Generic;
using System.Linq;
using RogueIslands.Boosters;

namespace RogueIslands
{
    public static class GameStateExtensions
    {
        private static readonly List<ConditionEvaluator> _evaluators = new()
        {
            new ProbabilityEvaluator(new Random()),
            new GameEventConditionEvaluator(),
            new BuildingCategoryScoredEvaluator(),
            new OrConditionEvaluator(),
            new TimeConditionEvaluator(),
        };

        private static readonly List<GameActionExecutor> _executors = new()
        {
            new MultiplierModifierExecutor(),
            new BoosterScalingExecutor(),
            new RetriggerScoringBuildingExecutor(),
            new PermanentBuildingUpgradeExecutor(),
            new GainSellValueExecutor(),
            new BoosterResetExecutor(),
            new DayModifierExecutor(),
        };

        public static void Execute(this GameState state, Booster booster, GameAction action)
        {
            _executors
                .First(x => x.ActionType.IsInstanceOfType(action))
                .Execute(state, booster, action);
        }

        public static bool IsConditionMet(this GameState state, IGameCondition condition)
        {
            return _evaluators
                .First(x => x.ConditionType.IsInstanceOfType(condition))
                .Evaluate(state, condition);
        }

        public static void ExecuteEventActions(this GameState state)
        {
            foreach (var booster in state.Boosters) 
                state.Execute(booster, booster.EventAction);
        }
    }
}