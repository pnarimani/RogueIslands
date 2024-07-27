using System;
using RogueIslands.Diagnostics;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.GameEvents;
using UnityEngine;
using UnityEngine.Pool;

namespace RogueIslands.Gameplay
{
    public interface IEventController
    {
        void Execute<T>(T e) where T : IGameEvent;
    }

    public class EventController : IEventController
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
            using var methodProfiler = new ProfilerBlock("EventController.Execute");
            using var methodEvent = new ProfilerBlock(typeof(T).Name);
            
            try
            {
                using var _ = ListPool<IBooster>.Get(out var buffer);

                buffer.Clear();
                buffer.AddRange(_state.Boosters);
                foreach (var booster in buffer)
                {
                    _state.CurrentEvent = e;
                    if (_state.Boosters.Contains((BoosterCard)booster))
                        ExecuteBooster(booster);
                }
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
            if (b.EventAction == null)
                return;

            if (_gameActionController.Execute(b, b.EventAction))
            {
                // Execute(new BoosterScoredEvent { Booster = b });
            }
        }
    }
}