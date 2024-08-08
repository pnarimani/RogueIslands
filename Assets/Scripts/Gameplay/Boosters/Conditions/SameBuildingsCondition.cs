using System.Linq;
using RogueIslands.Gameplay.Boosters.Sources;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.Boosters.Conditions
{
    public class SameBuildingsCondition : IGameConditionWithSource<Building>
    {
        public ISource<Building> Source { get; set; }
        public bool Evaluate(IBooster booster)
        {
            var buildings = Source.Get(booster).ToList();
            return buildings.Count <= 1 || buildings.All(other =>
            {
                var first = buildings[0];
                return first.Color == other.Color && first.Category == other.Category && first.Size == other.Size;
            });
        }
    }
}