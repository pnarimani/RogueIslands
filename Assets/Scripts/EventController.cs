using System;
using System.Collections.Generic;
using RogueIslands.Boosters;
using RogueIslands.GameEvents;
using UnityEngine;

namespace RogueIslands
{
    public class EventController
    {
        private readonly GameState _state;
        private readonly GameActionController _gameActionController;
        private readonly List<IBooster> _buffer = new();

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
                
                _buffer.Clear();
                _buffer.AddRange(_state.WorldBoosters);
                foreach (var booster in _buffer) 
                    ExecuteBooster(booster);
                
                _buffer.Clear();
                _buffer.AddRange(_state.Boosters);
                foreach (var booster in _buffer) 
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