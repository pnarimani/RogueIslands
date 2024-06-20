namespace RogueIslands.Boosters
{
    public class PermanentBuildingUpgradeExecutor : GameActionExecutor<PermanentBuildingUpgradeAction>
    {
        protected override void Execute(GameState state, IGameView view, BoosterCard booster,
            PermanentBuildingUpgradeAction action) 
            => state.ScoringState.SelectedBuilding.OutputUpgrade += action.ProductUpgrade;
    }
}