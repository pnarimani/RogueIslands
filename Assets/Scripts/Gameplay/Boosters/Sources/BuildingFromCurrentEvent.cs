using System;
using System.Collections.Generic;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.GameEvents;

namespace RogueIslands.Gameplay.Boosters.Sources
{
    public class BuildingFromCurrentEvent : ISource<Building>
    {
        public IEnumerable<Building> Get(GameState state, IBooster booster)
        {
            return state.CurrentEvent is BuildingEvent { Building: { } building }
                ? new[] { building }
                : ArraySegment<Building>.Empty;
        }
    }
}