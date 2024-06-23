using System;
using RogueIslands.GameEvents;
using UnityEngine;

namespace RogueIslands
{
    public static class EventExecution
    {
        public static void ExecuteEvent<T>(this GameState state, IGameView view, T e) where T : IGameEvent
        {
            StaticResolver.Resolve<EventController>().Execute(e);
        }
    }
}