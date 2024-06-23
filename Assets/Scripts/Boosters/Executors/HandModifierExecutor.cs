using RogueIslands.Boosters.Actions;

namespace RogueIslands.Boosters.Executors
{
    public class HandModifierExecutor : GameActionExecutor<HandModifier>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster, HandModifier action)
        {
            if (action.Change is { } change)
                state.HandSize += change;
            
            if (action.SetHandSize is { } size)
                state.HandSize = size;
        }
    }
}