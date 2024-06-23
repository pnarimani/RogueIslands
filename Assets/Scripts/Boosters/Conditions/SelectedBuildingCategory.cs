using System.Collections.Generic;
using RogueIslands.Buildings;

namespace RogueIslands.Boosters
{
    public class SelectedBuildingCategory : IGameCondition
    {
        public IReadOnlyList<Category> Categories { get; set; }
    }
}