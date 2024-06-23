using System.Collections.Generic;
using RogueIslands.Buildings;

namespace RogueIslands.Boosters
{
    public class BuildingColorCondition : IGameCondition
    {
        public IReadOnlyList<ColorTag> Colors { get; set; }
    }
}