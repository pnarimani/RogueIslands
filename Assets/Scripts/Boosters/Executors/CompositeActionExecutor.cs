using RogueIslands.Boosters.Actions;

namespace RogueIslands.Boosters.Executors
{
    public class CompositeActionExecutor : GameActionExecutor<CompositeAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster, CompositeAction action)
        {
            foreach (var subAction in action.Actions)
                state.Execute(view, booster, subAction);
        }
    }
}