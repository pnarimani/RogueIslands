using System;
using System.Collections.Generic;
using System.Linq;
using RogueIslands.Gameplay.GameEvents;

namespace RogueIslands.Gameplay.Boosters.Conditions
{
    public class GameEventCondition : IGameEventCondition
    {
        public GameEventCondition(params Type[] events) => TriggeringEvents = events;

        public GameEventCondition(IReadOnlyList<Type> triggeringEvents) => TriggeringEvents = triggeringEvents;
        public IReadOnlyList<Type> TriggeringEvents { get; set; }
        
        public GameEventCondition Or<TOther>() where TOther : IGameEvent =>
            new(TriggeringEvents.Append(typeof(TOther)).ToList());
    }

    public class GameEventCondition<T> : IGameEventCondition
    {
        public IReadOnlyList<Type> TriggeringEvents { get; set; } = new List<Type> { typeof(T) };

        public GameEventCondition Or<TOther>() where TOther : IGameEvent =>
            new(TriggeringEvents.Append(typeof(TOther)).ToList());
    }
}