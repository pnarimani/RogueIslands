using System.Collections.Generic;
using RogueIslands.Gameplay.Boosters.Sources;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.Boosters.Conditions
{
    public class BuildingSizeCondition : IGameConditionWithSource<Building>
    {
        public ISource<Building> Source { get; set; }
        public IReadOnlyList<BuildingSize> Allowed { get; set; }
        public IReadOnlyList<BuildingSize> Banned { get; set; }
    }
}