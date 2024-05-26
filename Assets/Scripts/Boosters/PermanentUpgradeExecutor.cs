namespace RogueIslands.Boosters
{
    public class PermanentUpgradeExecutor : GameActionExecutor<PermanentUpgradeAction>
    {
        protected override void Execute(GameState state, PermanentUpgradeAction action)
        {
            state.ScoringState.CurrentScoringBuilding.Building.OutputUpgrade += action.ProductUpgrade;
        }
    }
}