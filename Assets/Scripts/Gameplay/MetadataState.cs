using System.Collections.Generic;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay
{
    public class MetadataState
    {
        public Dictionary<Category, int> RunCategoryPlayCount { get; set; } = new();
        public Dictionary<BuildingSize, int> RoundSizePlayCount { get; set; } = new();
    }
}