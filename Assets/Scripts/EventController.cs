using System;
using RogueIslands.Boosters;
using RogueIslands.GameEvents;
using UnityEngine;

namespace RogueIslands
{
    public class EventController
    {
        private readonly GameState _state;
        private readonly GameActionController _gameActionController;

        public EventController(GameState state, GameActionController gameActionController)
        {
            _gameActionController = gameActionController;
            _state = state;
        }
        
        public void Execute<T>(T e) where T : IGameEvent
        {
            try
            {
                _state.CurrentEvent = e;
                foreach (var booster in _state.WorldBoosters) 
                    ExecuteBooster(booster);
                foreach (var booster in _state.Boosters) 
                    ExecuteBooster(booster);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
            finally
            {
                _state.CurrentEvent = null;
            }
        }

        private void ExecuteBooster(IBooster b)
        {
            if (b.EventAction != null)
                _gameActionController.Execute(b, b.EventAction);
        }
    }
}