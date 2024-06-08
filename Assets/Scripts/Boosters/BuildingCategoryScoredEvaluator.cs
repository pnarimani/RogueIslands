namespace RogueIslands.Boosters
{
    public class BuildingCategoryScoredEvaluator : ConditionEvaluator<BuildingCategoryScoredCondition>
    {
        protected override bool Evaluate(GameState state, BuildingCategoryScoredCondition condition) 
            => state.ScoringState is { CurrentScoringBuilding: not null } && state.ScoringState.CurrentScoringBuilding.Category == condition.Category;
    }
}