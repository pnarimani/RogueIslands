namespace RogueIslands.Boosters
{
    public class CompositeActionExecutor : GameActionExecutor<CompositeAction>
    {
        protected override void Execute(GameState state, IGameView view, Booster booster, CompositeAction action)
        {
            foreach (var subAction in action.Actions)
                state.Execute(view, booster, subAction);
        }
    }
}