using System.Collections.Generic;

namespace RogueIslands.Boosters.Actions
{
    public abstract class GameAction
    {
        public IReadOnlyList<IGameCondition> Conditions { get; set; }
    }
}