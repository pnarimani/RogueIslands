using System.Collections.Generic;

namespace RogueIslands.Boosters.Actions
{
    public class CompositeAction : GameAction
    {
        public IReadOnlyList<GameAction> Actions { get; set; }
    }
}