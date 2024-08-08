using System.Collections.Generic;

namespace RogueIslands.Gameplay.Boosters.Actions
{
    public class CompositeAction : GameAction
    {
        public IReadOnlyList<GameAction> Actions { get; set; }

        protected override bool ExecuteAction(IBooster booster)
        {
            var result = false;
            foreach (var action in Actions)
                result |= action.Execute(booster);
            return result;
        }
    }
}