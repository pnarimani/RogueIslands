using RogueIslands.Gameplay.Boosters.Actions;

namespace RogueIslands.Gameplay.Boosters.Executors
{
    public class BoosterResetExecutor : GameActionExecutor<BoosterResetAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster, BoosterResetAction action) 
            => ScaleScoringAction(action, booster.GetEventAction<ScoringAction>());

        private static void ScaleScoringAction(BoosterResetAction action, ScoringAction scoringAction)
        {
            scoringAction.Products = action.Product;
            scoringAction.Multiplier = action.Multiplier;
        }
    }
}