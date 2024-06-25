using System.Collections.Generic;
using RogueIslands.Buildings;

namespace RogueIslands.Boosters.Conditions
{
    public class BuildingCategoryCondition : IGameCondition
    {
        public IReadOnlyList<Category> Categories { get; set; }
    }
}