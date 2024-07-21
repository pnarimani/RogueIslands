using System;
using RogueIslands.Gameplay.Boosters.Actions;
using RogueIslands.Gameplay.GameEvents;
using RogueIslands.Serialization;

namespace RogueIslands.Gameplay.Boosters.Executors
{
    public class CopyBoosterExecutor : GameActionExecutor
    {
        private readonly GameActionController _gameActionController;
        private readonly ICloner _cloner;

        public CopyBoosterExecutor(GameActionController gameActionController, ICloner cloner)
        {
            _cloner = cloner;
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

            var copyBooster = (CopyBoosterAction)action;

            if (state.CurrentEvent is ResetRetriggers)
            {
                copyBooster.Cloned = _cloner.Clone(nextBooster.EventAction);
                return false;
            }

            if (copyBooster.Cloned != null)
                return _gameActionController.Execute(booster, copyBooster.Cloned);
            
            return false;
        }
    }
}