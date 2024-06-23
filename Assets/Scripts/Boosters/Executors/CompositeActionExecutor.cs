using RogueIslands.Boosters.Actions;

namespace RogueIslands.Boosters.Executors
{
    public class CompositeActionExecutor : GameActionExecutor<CompositeAction>
    {
        private GameActionController _actionController;

        public CompositeActionExecutor(GameActionController actionController)
        {
            _actionController = actionController;
        }
        protected override void Execute(GameState state, IGameView view, IBooster booster, CompositeAction action)
        {
            foreach (var subAction in action.Actions)
                _actionController.Execute(booster, subAction);
        }
    }
}