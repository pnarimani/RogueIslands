using RogueIslands.Gameplay.Boosters.Actions;

namespace RogueIslands.Gameplay.Boosters.Executors
{
    public class ChangeMoneyExecutor : GameActionExecutor<ChangeMoneyAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster, ChangeMoneyAction action)
        {
            if (action.IsImmediate)
            {
                state.Money += action.Change;
                
                view.GetBooster(booster).GetMoneyVisualizer().Play(action.Change);
            }
            else
            {
                state.MoneyChanges.Add(new MoneyChange
                {
                    Reason = booster.Name,
                    Change = action.Change,
                });
            }
        }
    }
}