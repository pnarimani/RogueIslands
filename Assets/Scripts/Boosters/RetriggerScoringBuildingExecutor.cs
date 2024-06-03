namespace RogueIslands.Boosters
{
    public class RetriggerScoringBuildingExecutor : GameActionExecutor<RetriggerScoringBuildingAction>
    {
        protected override void Execute(GameState state, IGameView view, Booster booster,
            RetriggerScoringBuildingAction action)
            => state.ScoringState.CurrentScoringBuilding.RemainingTriggers += action.RetriggerTimes;
    }
}