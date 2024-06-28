using System.Collections.Generic;
using RogueIslands.Gameplay.Boosters.Conditions;

namespace RogueIslands.Gameplay.Boosters.Actions
{
    public abstract class GameAction
    {
        public IReadOnlyList<IGameCondition> Conditions { get; set; }
    }
}