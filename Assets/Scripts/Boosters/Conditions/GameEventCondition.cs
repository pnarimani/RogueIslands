using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RogueIslands.GameEvents;

namespace RogueIslands
{
    public class GameEventCondition : IGameCondition
    {
        public IReadOnlyList<Type> TriggeringEvents { get; }

        public GameEventCondition(Type triggeringEvent)
            => TriggeringEvents = new List<Type> { triggeringEvent };

        [JsonConstructor]
        public GameEventCondition(IReadOnlyList<Type> triggeringEvents)
            => TriggeringEvents = triggeringEvents;
        
        public static GameEventCondition Create<T>() where T : IGameEvent => new(typeof(T));
    }
}