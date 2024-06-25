using System.Collections.Generic;
using RogueIslands.Buildings;

namespace RogueIslands.Boosters.Conditions
{
    public class BuildingColorCondition : IGameCondition
    {
        public IReadOnlyList<ColorTag> Colors { get; set; }
    }
}