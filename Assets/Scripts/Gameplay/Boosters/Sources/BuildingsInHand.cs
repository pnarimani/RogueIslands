using System.Collections.Generic;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.Boosters.Sources
{
    public class BuildingsInHand : ISource<Building>
    {
        public IEnumerable<Building> Get(GameState state, IBooster booster) => state.BuildingsInHand;
    }
}