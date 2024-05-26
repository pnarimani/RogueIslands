using System.Collections.Generic;

namespace RogueIslands.Boosters
{
    public abstract class GameAction
    {
        public IReadOnlyList<IGameCondition> Conditions { get; set; }
    }
}