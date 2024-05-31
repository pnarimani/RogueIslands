namespace RogueIslands.Boosters
{
    public class CompositeActionExecutor : GameActionExecutor<CompositeAction>
    {
        protected override void Execute(GameState state, Booster booster, CompositeAction action)
        {
            foreach (var subAction in action.Actions)
                state.Execute(booster, subAction);
        }
    }
}