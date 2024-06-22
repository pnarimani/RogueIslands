using RogueIslands.Boosters.Actions;

namespace RogueIslands.Boosters.Executors
{
    public class BoosterResetExecutor : GameActionExecutor<BoosterResetAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster, BoosterResetAction action) 
            => ScaleScoringAction(action, booster.GetEventAction<ScoringAction>());

        private static void ScaleScoringAction(BoosterResetAction action, ScoringAction scoringAction)
        {
            scoringAction.Products = action.Product;
            scoringAction.PlusMult = action.PlusMult;
            scoringAction.XMult = action.XMult;
        }
    }
}