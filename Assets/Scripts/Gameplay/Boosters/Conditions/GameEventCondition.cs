using System;
using System.Collections.Generic;
using System.Linq;
using RogueIslands.Gameplay.GameEvents;
using UnityEditorInternal.Profiling.Memory.Experimental;

namespace RogueIslands.Gameplay.Boosters.Conditions
{
    public class GameEventCondition : IGameCondition
    {
        public IReadOnlyList<Type> TriggeringEvents { get; set; }

        public static GameEventCondition Create<T>() where T : IGameEvent
            => new()
            {
                TriggeringEvents = new List<Type>() { typeof(T) },
            };

        public GameEventCondition Or<TOther>() where TOther : IGameEvent
        {
            TriggeringEvents = TriggeringEvents.Append(typeof(TOther)).ToList();
            return this;
        }
    }
}