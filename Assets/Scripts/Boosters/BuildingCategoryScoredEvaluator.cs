namespace RogueIslands.Boosters
{
    public class BuildingCategoryScoredEvaluator : ConditionEvaluator<BuildingCategoryScoredCondition>
    {
        protected override bool Evaluate(GameState state, BuildingCategoryScoredCondition condition) 
            => state.ScoringState.CurrentScoringBuilding.Building.Category == condition.Category;
    }
}