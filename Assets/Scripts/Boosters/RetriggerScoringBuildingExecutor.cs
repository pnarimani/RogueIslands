namespace RogueIslands.Boosters
{
    public class RetriggerScoringBuildingExecutor : GameActionExecutor<RetriggerScoringBuildingAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster,
            RetriggerScoringBuildingAction action)
        {
            if(state.ScoringState.SelectedBuilding == null)
                return;
            
            state.ScoringState.SelectedBuilding.RemainingTriggers += action.RetriggerTimes;
        }
    }
}