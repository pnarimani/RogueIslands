namespace RogueIslands.Boosters
{
    public class RetriggerScoringBuildingExecutor : GameActionExecutor<RetriggerScoringBuildingAction>
    {
        protected override void Execute(GameState state, IGameView view, Booster booster, RetriggerScoringBuildingAction action)
        {
            if(state.ScoringState.CurrentScoringBuilding == null)
                return;
            
            state.ScoringState.CurrentScoringBuilding.RemainingTriggers += action.RetriggerTimes;
        }
    }
}