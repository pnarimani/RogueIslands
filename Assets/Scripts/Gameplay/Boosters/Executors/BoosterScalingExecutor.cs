using RogueIslands.Gameplay.Boosters.Actions;

namespace RogueIslands.Gameplay.Boosters.Executors
{
    public class BoosterScalingExecutor : GameActionExecutor<BoosterScalingAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster, BoosterScalingAction action)
            => ScaleScoringAction(action, booster.GetEventAction<ScoringAction>());

        private static void ScaleScoringAction(BoosterScalingAction action, ScoringAction scoringAction)
        {
            if (action.ProductChange is { } p)
                scoringAction.Products += p;
            if (action.PlusMultChange is { } q)
                scoringAction.PlusMult += q;
            if (action.XMultChange is { } r)
                scoringAction.XMult += r;
        }
    }
}