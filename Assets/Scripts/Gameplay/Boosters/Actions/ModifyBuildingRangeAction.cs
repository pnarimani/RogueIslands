using RogueIslands.Gameplay.Boosters.Sources;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.Boosters.Actions
{
    public class ModifyBuildingRangeAction : GameAction
    {
        public ISource<Building> Source { get; set; }
        public float RangeMultiplier { get; set; }
    }
}