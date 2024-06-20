namespace RogueIslands.Boosters
{
    public class SelectedBuildingCategoryEvaluator : ConditionEvaluator<SelectedBuildingCategory>
    {
        protected override bool Evaluate(GameState state, SelectedBuildingCategory condition) 
            => state.ScoringState is { SelectedBuilding: not null } && state.ScoringState.SelectedBuilding.Category == condition.Category;
    }
}