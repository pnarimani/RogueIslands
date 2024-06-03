namespace RogueIslands.Boosters
{
    public class DayModifierExecutor : GameActionExecutor<DayModifier>
    {
        protected override void Execute(GameState state, IGameView view, Booster booster, DayModifier action)
        {
            if (action.ChangeDays != 0)
                state.TotalDays += action.ChangeDays;
            else if (action.SetDays.HasValue)
                state.TotalDays = action.SetDays.Value;
        }
    }
}