using RogueIslands.Boosters.Actions;

namespace RogueIslands.Boosters.Executors
{
    public class CompositeActionExecutor : GameActionExecutor<CompositeAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster, CompositeAction action)
        {
            var actionManager = StaticResolver.Resolve<GameActionController>();
            foreach (var subAction in action.Actions)
                actionManager.Execute(booster, subAction);
        }
    }
}