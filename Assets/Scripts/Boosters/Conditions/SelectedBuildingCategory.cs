using System.Collections.Generic;

namespace RogueIslands.Boosters
{
    public class SelectedBuildingCategory : IGameCondition
    {
        public IReadOnlyList<Category> Categories { get; set; }
    }
}