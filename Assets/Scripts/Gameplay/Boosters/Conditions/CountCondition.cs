using RogueIslands.Gameplay.Boosters.Sources;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.Boosters.Conditions
{
    public class CountCondition : IGameConditionWithSource<int>
    {
        public enum Mode
        {
            Less,
            More,
            Equal,
            Even,
            Odd,
            PowerOfTwo,
        }
        
        public ISource<int> Source { get; set; }
        public Mode ComparisonMode { get; set; }
        public int Value { get; set; }
    }
}