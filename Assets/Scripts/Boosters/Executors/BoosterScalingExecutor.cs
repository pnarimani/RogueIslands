using RogueIslands.Boosters.Actions;

namespace RogueIslands.Boosters.Executors
{
    public class BoosterScalingExecutor : GameActionExecutor<BoosterScalingAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster, BoosterScalingAction action) 
            => ScaleScoringAction(action, booster.GetEventAction<ScoringAction>());

        private static void ScaleScoringAction(BoosterScalingAction action, ScoringAction scoringAction)
        {
            scoringAction.Products += action.ProductChange;
            scoringAction.PlusMult += action.PlusMultChange;
            scoringAction.XMult += action.XMultChange;
            
            scoringAction.Products = scoringAction.Products < 0 ? 0 : scoringAction.Products;
            scoringAction.PlusMult = scoringAction.PlusMult < 0 ? 0 : scoringAction.PlusMult;
            scoringAction.XMult = scoringAction.XMult < 1 ? 1 : scoringAction.XMult;
        }
    }
}