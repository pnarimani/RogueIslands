using System;
using RogueIslands.Gameplay.Boosters.Actions;

namespace RogueIslands.Gameplay.Boosters.Executors
{
    public class ScaleChangeMoneyActionExecutor : GameActionExecutor<ScaleChangeMoneyAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster,
            ScaleChangeMoneyAction action)
        {
            var money = booster.GetEventAction<ChangeMoneyAction>();
            if (action.Change is { } ch)
                money.Change += ch;
            if(action.Set is {} set)
                money.Change = set;
            money.Change = Math.Max(0, money.Change);
        }
    }
}