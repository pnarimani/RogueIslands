using RogueIslands.Gameplay.Boosters.Actions;

namespace RogueIslands.Gameplay.Boosters.Executors
{
    public class BoosterResetExecutor : GameActionExecutor<BoosterResetAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster, BoosterResetAction action)
        {
            var scoringAction = booster.GetEventAction<ScoringAction>();
            scoringAction.Addition = action.Product;
            scoringAction.Multiplier = action.Multiplier;

            view.GetBooster(booster.Id).GetResetVisualizer().PlayReset();
        }
    }
}