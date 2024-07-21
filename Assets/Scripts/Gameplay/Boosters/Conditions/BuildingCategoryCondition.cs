using System.Collections.Generic;
using RogueIslands.Gameplay.Boosters.Sources;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.Boosters.Conditions
{
    public class BuildingCategoryCondition : IGameConditionWithSource<Building>
    {
        public ISource<Building> Source { get; set; }
        public IReadOnlyList<Category> Categories { get; set; }
    }
}