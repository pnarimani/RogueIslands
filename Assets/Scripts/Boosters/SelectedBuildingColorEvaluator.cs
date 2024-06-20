namespace RogueIslands.Boosters
{
    public class SelectedBuildingColorEvaluator : ConditionEvaluator<SelectedBuildingColorCondition>
    {
        protected override bool Evaluate(GameState state, SelectedBuildingColorCondition condition) 
            => state.ScoringState.SelectedBuilding is { } building && building.Color == condition.ColorTag;
    }
}