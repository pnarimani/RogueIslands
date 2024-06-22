using System;
using System.Collections.Generic;
using System.Linq;
using RogueIslands.Boosters;
using RogueIslands.Boosters.Actions;
using RogueIslands.Boosters.Executors;
using UnityEngine.Assertions;

namespace RogueIslands
{
    public static class GameActionManager
    {
        private static IReadOnlyList<GameActionExecutor> _defaultExecutors;

        public static void Execute(this GameState state, IGameView view, IBooster booster, GameAction action)
        {
            Assert.IsNotNull(action);
            Assert.IsNotNull(booster);
            Assert.IsNotNull(view);

            _defaultExecutors ??= StaticResolver.Resolve<IReadOnlyList<GameActionExecutor>>();

            Assert.IsNotNull(_defaultExecutors);

            var exec = _defaultExecutors.FirstOrDefault(x => x.ActionType == action.GetType());
            if (exec == null)
                throw new InvalidOperationException($"No executor found for action type {action.GetType().Name}");
            exec.Execute(state, view, booster, action);
        }
    }
}