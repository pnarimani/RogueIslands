using System;
using RogueIslands.Gameplay.Boosters.Actions;

namespace RogueIslands.Gameplay.Boosters.Executors
{
    public class CompositeActionExecutor : GameActionExecutor
    {
        private GameActionController _actionController;

        public override Type ActionType { get; } = typeof(CompositeAction);

        public CompositeActionExecutor(GameActionController actionController)
            => _actionController = actionController;

        public override bool Execute(GameState state, IGameView view, IBooster booster, GameAction action)
        {
            var result = false;
            foreach (var subAction in ((CompositeAction)action).Actions)
                result |= _actionController.Execute(booster, subAction);
            return result;
        }
    }
}