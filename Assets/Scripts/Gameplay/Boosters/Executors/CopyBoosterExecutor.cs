using System;
using RogueIslands.Gameplay.Boosters.Actions;

namespace RogueIslands.Gameplay.Boosters.Executors
{
    public class CopyBoosterExecutor : GameActionExecutor
    {
        private readonly GameActionController _gameActionController;

        public CopyBoosterExecutor(GameActionController gameActionController)
        {
            _gameActionController = gameActionController;
        }

        public override Type ActionType { get; } = typeof(CopyBoosterAction);

        public override bool Execute(GameState state, IGameView view, IBooster booster, GameAction action)
        {
            var index = state.Boosters.IndexOf((BoosterCard)booster);
            if (index >= state.Boosters.Count - 1)
                return false;
            
            var nextBooster = state.Boosters[index + 1];
            if (nextBooster.EventAction == null)
                return false;
            return _gameActionController.Execute(booster, nextBooster.EventAction);
        }
    }
}