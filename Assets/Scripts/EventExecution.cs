using System;
using RogueIslands.GameEvents;
using UnityEngine;

namespace RogueIslands
{
    public static class EventExecution
    {
        public static void ExecuteEvent<T>(this GameState state, IGameView view, T e) where T : IGameEvent
        {
            try
            {
                var gameActionManager = StaticResolver.Resolve<GameActionController>();
                state.CurrentEvent = e;
                foreach (var worldBooster in state.WorldBoosters)
                {
                    if (worldBooster.EventAction != null)
                        gameActionManager.Execute(worldBooster, worldBooster.EventAction);
                }

                foreach (var booster in state.Boosters)
                {
                    if (booster.EventAction != null)
                        gameActionManager.Execute(booster, booster.EventAction);
                }
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
            finally
            {
                state.CurrentEvent = null;
            }
        }
    }
}