using RogueIslands.Gameplay.Boosters.Sources;

namespace RogueIslands.Gameplay.Boosters.Actions
{
    public class ChangeMoneyAction : GameAction
    {
        public bool IsImmediate { get; set; }
        public int Change { get; set; }
        public ISource<int> Multiplier { get; set; }
    }
}