using System;

namespace RogueIslands.Gameplay.Boosters.Actions
{
    public class ScaleChangeMoneyAction : GameAction
    {
        public int? Change { get; set; }
        public int? Set { get; set; }
        protected override bool ExecuteAction(IBooster booster)
        {
            var money = booster.GetEventAction<ChangeMoneyAction>();
            if (Change is { } ch)
                money.Change += ch;
            if(Set is {} set)
                money.Change = set;
            money.Change = Math.Max(0, money.Change);
            return true;
        }
    }
}