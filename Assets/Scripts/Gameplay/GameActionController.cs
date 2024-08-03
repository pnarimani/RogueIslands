using System;
using System.Collections.Generic;
using System.Linq;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.Boosters.Actions;
using RogueIslands.Gameplay.Boosters.Executors;
using UnityEngine;

namespace RogueIslands.Gameplay
{
    public class GameActionController
    {
        private readonly Lazy<IReadOnlyList<GameActionExecutor>> _execs;
        private readonly GameState _state;
        private readonly IGameView _view;
        private readonly GameConditionsController _conditionsController;

        public GameActionController(GameState state, IGameView view, GameConditionsController conditionsController,
            Lazy<IReadOnlyList<GameActionExecutor>> execs)
        {
            _conditionsController = conditionsController;
            _view = view;
            _state = state;
            _execs = execs;
        }

        public bool Execute(IBooster booster, GameAction action)
        {
            if (action.Condition != null && !_conditionsController.IsConditionMet(booster, action.Condition))
                return false;

            var exec = _execs.Value.FirstOrDefault(x => x.ActionType == action.GetType());
            if (exec == null)
                throw new InvalidOperationException($"No executor found for action type {action.GetType().Name}");
            return exec.Execute(_state, _view, booster, action);
        }
    }
}