using System.Linq;
using RogueIslands.Gameplay.Boosters.Actions;

namespace RogueIslands.Gameplay.Boosters.Executors
{
    public class ChangeMoneyExecutor : GameActionExecutor<ChangeMoneyAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster, ChangeMoneyAction action)
        {
            var mult = 1;
            if (action.Multiplier != null)
                mult = action.Multiplier.Get(state, booster).First();

            var finalChange = action.Change * mult;
            if (finalChange == 0)
                return;
            
            if (action.IsImmediate)
            {
                state.Money += finalChange;
                view.GetBooster(booster.Id).GetMoneyVisualizer().Play(finalChange);
            }
            else
            {
                state.MoneyChanges.Add(new MoneyChange
                {
                    Reason = booster.Name,
                    Change = finalChange,
                });
            }
        }
    }
}