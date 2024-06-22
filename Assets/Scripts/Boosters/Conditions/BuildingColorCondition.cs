using System.Collections.Generic;

namespace RogueIslands.Boosters
{
    public class BuildingColorCondition : IGameCondition
    {
        public IReadOnlyList<ColorTag> Colors { get; set; }
    }
}