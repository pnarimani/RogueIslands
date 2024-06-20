namespace RogueIslands.Boosters
{
    public class DayModifierExecutor : GameActionExecutor<DayModifier>
    {
        protected override void Execute(GameState state, IGameView view, BoosterCard booster, DayModifier action)
        {
            if (action.ShouldSetToDefault)
            {
                state.TotalDays = state.DefaultTotalDays;
            }
            else
            {
                if (action.Change != 0)
                    state.TotalDays += action.Change;
                else if (action.SetDays.HasValue)
                    state.TotalDays = action.SetDays.Value;
            }
        }
    }
}