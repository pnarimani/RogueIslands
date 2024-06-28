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
        private IReadOnlyList<GameActionExecutor> _execs;
        private readonly GameState _state;
        private readonly IGameView _view;
        private readonly GameConditionsController _conditionsController;

        public GameActionController(GameState state, IGameView view, GameConditionsController conditionsController)
        {
            _conditionsController = conditionsController;
            _view = view;
            _state = state;
        }
        
        public void SetExecutors(IReadOnlyList<GameActionExecutor> execs)
        {
            _execs = execs;
        }
        
        public void Execute(IBooster booster, GameAction action)
        {
            try
            {
                if (action.Conditions != null && action.Conditions.Any(condition => !_conditionsController.IsConditionMet(booster, condition)))
                    return;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to evaluate conditions for action `{action.GetType().Name}` on booster {booster.Id}");
                Debug.LogException(e);
                return;
            }
            
            var exec = _execs.FirstOrDefault(x => x.ActionType == action.GetType());
            if (exec == null)
                throw new InvalidOperationException($"No executor found for action type {action.GetType().Name}");
            exec.Execute(_state, _view, booster, action);
        }
    }
}