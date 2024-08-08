using System;
using System.Collections.Generic;
using RogueIslands.Autofac;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.GameEvents;

namespace RogueIslands.Gameplay.Boosters.Sources
{
    public class BuildingFromCurrentEvent : ISource<Building>
    {
        public IEnumerable<Building> Get(IBooster booster)
        {
            var state = StaticResolver.Resolve<GameState>();
            return state.CurrentEvent is BuildingEvent { Building: { } building }
                ? new[] { building }
                : ArraySegment<Building>.Empty;
        }
    }
}