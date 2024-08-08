using System;
using System.Collections.Generic;
using System.Linq;
using RogueIslands.Autofac;
using RogueIslands.Gameplay.GameEvents;

namespace RogueIslands.Gameplay.Boosters.Conditions
{
    public class GameEventCondition : IGameCondition
    {
        public GameEventCondition(params Type[] events)
        {
            TriggeringEvents = events;
        }

        public GameEventCondition(IReadOnlyList<Type> triggeringEvents)
        {
            TriggeringEvents = triggeringEvents;
        }

        public IReadOnlyList<Type> TriggeringEvents { get; set; }

        public GameEventCondition Or<TOther>() where TOther : IGameEvent
        {
            return new GameEventCondition(TriggeringEvents.Append(typeof(TOther)).ToList());
        }

        public bool Evaluate(IBooster booster)
        {
            var state = StaticResolver.Resolve<GameState>();
            foreach (var e in TriggeringEvents)
                if (e.IsInstanceOfType(state.CurrentEvent))
                    return true;

            return false;
        }
    }
}