namespace RogueIslands.Boosters
{
    public class BadEyesConditionEvaluator : ConditionEvaluator<SelectedBuildingCategory>, IEvaluationConditionOverride
    {
        protected override bool Evaluate(GameState state, IBooster booster, SelectedBuildingCategory condition)
        {
            if (state.ScoringState is not { SelectedBuilding: not null })
                return false;
            
            var buildingCategory = state.ScoringState.SelectedBuilding.Category;
            var conditionCategory = condition.Category;

            if (conditionCategory == Category.Cat1)
                conditionCategory = Category.Cat3;
            if (conditionCategory == Category.Cat2)
                conditionCategory = Category.Cat4;

            if (buildingCategory == Category.Cat1)
                buildingCategory = Category.Cat3;
            if (buildingCategory == Category.Cat2)
                buildingCategory = Category.Cat4;

            return buildingCategory == conditionCategory;

        }
    }
}