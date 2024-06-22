using RogueIslands.Boosters.Actions;

namespace RogueIslands.Boosters.Executors
{
    public class DayModifierExecutor : GameActionExecutor<DayModifier>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster, DayModifier action)
        {
            if (action.Change != 0)
                state.TotalDays += action.Change;
            else if (action.SetDays.HasValue)
                state.TotalDays = action.SetDays.Value;
        }
    }
}