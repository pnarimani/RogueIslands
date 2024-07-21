using System.Collections.Generic;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.Boosters.Conditions
{
    public class BuildingSizeCondition : IGameCondition
    {
        public IReadOnlyList<BuildingSize> Allowed { get; set; }
        public IReadOnlyList<BuildingSize> Banned { get; set; }
    }
}