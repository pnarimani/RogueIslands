using System.Collections.Generic;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.Boosters.Conditions
{
    public class BuildingCategoryCondition : IGameCondition
    {
        public IReadOnlyList<Category> Categories { get; set; }
    }
}