namespace RogueIslands.Boosters
{
    public class PermanentBuildingUpgradeExecutor : GameActionExecutor<PermanentBuildingUpgradeAction>
    {
        protected override void Execute(GameState state, IGameView view, Booster booster,
            PermanentBuildingUpgradeAction action) 
            => state.ScoringState.CurrentScoringBuilding.OutputUpgrade += action.ProductUpgrade;
    }
}