using System.Collections.Generic;
using RogueIslands.Buildings;

namespace RogueIslands.Boosters
{
    public class BuildingCategoryCondition : IGameCondition
    {
        public IReadOnlyList<Category> Categories { get; set; }
    }
}