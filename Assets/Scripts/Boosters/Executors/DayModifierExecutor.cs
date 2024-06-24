using RogueIslands.Boosters.Actions;

namespace RogueIslands.Boosters.Executors
{
    public class DayModifierExecutor : GameActionExecutor<DayModifier>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster, DayModifier action)
        {
            if (action.Change is { } change)
                state.TotalDays += change;
            if (action.SetDays is { } set)
                state.TotalDays = set;
        }
    }
}