using System;
using System.Collections.Generic;
using System.Linq;
using RogueIslands.Gameplay.GameEvents;

namespace RogueIslands.Gameplay.Boosters.Conditions
{
    public class GameEventCondition : IGameEventCondition
    {
        public IReadOnlyList<Type> TriggeringEvents { get; set; }

        public GameEventCondition(params Type[] events) => TriggeringEvents = events;

        public GameEventCondition(IReadOnlyList<Type> triggeringEvents) => TriggeringEvents = triggeringEvents;
    }

    public class GameEventCondition<T> : IGameEventCondition
    {
        public IReadOnlyList<Type> TriggeringEvents { get; set; } = new List<Type> { typeof(T) };

        public GameEventCondition<T> Or<TOther>() where TOther : IGameEvent
        {
            TriggeringEvents = TriggeringEvents.Append(typeof(TOther)).ToList();
            return this;
        }
    }
}