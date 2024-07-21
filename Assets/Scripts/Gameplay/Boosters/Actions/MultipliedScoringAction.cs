using RogueIslands.Gameplay.Boosters.Sources;

namespace RogueIslands.Gameplay.Boosters.Actions
{
    public class MultipliedScoringAction : ScoringAction
    {
        public ISource<int> Factor { get; set; }
    }
}