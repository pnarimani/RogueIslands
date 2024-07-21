using RogueIslands.Gameplay.Boosters.Sources;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.Boosters.Conditions
{
    public class SameBuildingsCondition : IGameConditionWithSource<Building>
    {
        public ISource<Building> Source { get; set; }
    }
}