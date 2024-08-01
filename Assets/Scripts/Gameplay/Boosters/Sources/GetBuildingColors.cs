using System.Collections.Generic;
using System.Linq;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.Boosters.Sources
{
    public class GetBuildingColors : ISource<ColorTag>
    {
        public ISource<Building> Source { get; set; }

        public IEnumerable<ColorTag> Get(GameState state, IBooster booster) =>
            Source.Get(state, booster).Select(b => b.Color);
    }
}