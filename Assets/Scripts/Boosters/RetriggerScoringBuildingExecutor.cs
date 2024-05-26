namespace RogueIslands.Boosters
{
    public class RetriggerScoringBuildingExecutor : GameActionExecutor<RetriggerScoringBuildingAction>
    {
        protected override void Execute(GameState state, RetriggerScoringBuildingAction action)
            => state.ScoringState.CurrentScoringBuilding.RemainingTriggers += action.RetriggerTimes;
    }
}