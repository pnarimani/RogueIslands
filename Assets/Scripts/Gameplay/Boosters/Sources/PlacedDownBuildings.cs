using System.Collections.Generic;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.Boosters.Sources
{
    public class PlacedDownBuildings : ISource<Building>
    {
        public IEnumerable<Building> Get(GameState state, IBooster booster)
        {
            return state.PlacedDownBuildings;
        }
    }
}