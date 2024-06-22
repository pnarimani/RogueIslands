using System.Collections.Generic;

namespace RogueIslands.Boosters
{
    public class CompositeAction : GameAction
    {
        public IReadOnlyList<GameAction> Actions { get; set; }
    }
}