using System;
using System.Collections.Generic;
using RogueIslands.GameEvents;

namespace RogueIslands.Boosters.Conditions
{
    public class GameEventCondition : IGameCondition
    {
        public IReadOnlyList<Type> TriggeringEvents { get; set; }

        public static GameEventCondition Create<T>() where T : IGameEvent
            => new()
            {
                TriggeringEvents = new List<Type>() { typeof(T) },
            };
    }
}