using System.Collections.Generic;

namespace RogueIslands.Boosters
{
    public class SelectedBuildingColorCondition : IGameCondition
    {
        public IReadOnlyList<ColorTag> Colors { get; set; }
    }
}