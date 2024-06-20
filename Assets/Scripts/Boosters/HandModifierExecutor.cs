namespace RogueIslands.Boosters
{
    public class HandModifierExecutor : GameActionExecutor<HandModifier>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster, HandModifier action)
        {
            if (action.ShouldSetToDefault)
            {
                state.HandSize = state.DefaultHandSize;
            }
            else
            {
                if (action.Change != 0)
                    state.HandSize += action.Change;
                else if (action.SetHandSize.HasValue)
                    state.HandSize = action.SetHandSize.Value;
            }
        }
    }
}