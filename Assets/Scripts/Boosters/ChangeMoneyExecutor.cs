namespace RogueIslands.Boosters
{
    public class ChangeMoneyExecutor : GameActionExecutor<ChangeMoneyAction>
    {
        protected override void Execute(GameState state, IGameView view, Booster booster, ChangeMoneyAction action)
        {
            state.MoneyChanges.Add(new MoneyChange
            {
                Reason = booster.Name,
                Change = action.Change,
            });
        }
    }
}