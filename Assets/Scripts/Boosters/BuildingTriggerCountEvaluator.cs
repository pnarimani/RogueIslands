namespace RogueIslands.Boosters
{
    public class BuildingTriggerCountEvaluator : ConditionEvaluator<BuildingTriggerCountCheck>
    {
        protected override bool Evaluate(GameState state, IBooster booster, BuildingTriggerCountCheck condition) 
            => state.ScoringState.SelectedBuilding is not null && state.ScoringState.SelectedBuildingTriggerCount == condition.TriggerCount;
    }
}