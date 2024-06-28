using RogueIslands.Gameplay.Boosters.Actions;

namespace RogueIslands.Gameplay.Boosters.Executors
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