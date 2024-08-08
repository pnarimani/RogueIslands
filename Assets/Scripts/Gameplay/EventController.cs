﻿using System;
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

        public EventController(GameState state)
        {
            _state = state;
        }

        public void Execute<T>(T e) where T : IGameEvent
        {
            using var methodProfiler = new ProfilerScope("EventController.Execute");
            using var methodEvent = new ProfilerScope(typeof(T).Name);
            
            try
            {
                using var _ = ListPool<IBooster>.Get(out var buffer);

                buffer.Clear();
                buffer.AddRange(_state.Boosters);
                foreach (var booster in buffer)
                {
                    _state.CurrentEvent = e;
                    if (_state.Boosters.Contains((BoosterCard)booster))
                        booster.EventAction?.Execute(booster);
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
    }
}