namespace RogueIslands.Boosters
{
    public class PermanentBuildingUpgradeExecutor : GameActionExecutor<PermanentBuildingUpgradeAction>
    {
        protected override void Execute(GameState state, Booster booster, PermanentBuildingUpgradeAction action) 
            => state.ScoringState.CurrentScoringBuilding.Building.OutputUpgrade += action.ProductUpgrade;
    }
}