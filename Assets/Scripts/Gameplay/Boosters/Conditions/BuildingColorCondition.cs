using System.Collections.Generic;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.Boosters.Conditions
{
    public class BuildingColorCondition : IGameCondition
    {
        public IReadOnlyList<ColorTag> Colors { get; set; }
    }
}