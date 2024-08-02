using System.Collections.Generic;

namespace RogueIslands.Gameplay.Boosters.Actions
{
    public class CompositeAction : GameAction
    {
        public IReadOnlyList<GameAction> Actions { get; set; }

        public static implicit operator CompositeAction(GameAction[] actions) => new() { Actions = actions };
    }
}