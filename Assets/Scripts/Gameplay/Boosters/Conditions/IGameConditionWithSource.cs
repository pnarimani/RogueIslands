using RogueIslands.Gameplay.Boosters.Sources;

namespace RogueIslands.Gameplay.Boosters.Conditions
{
    public interface IGameConditionWithSource<T> : IGameCondition
    {
        public ISource<T> Source { get; set; }
    }
}