namespace RogueIslands.Boosters
{
    public class BuildingCategoryScoredEvaluator : ConditionEvaluator<BuildingCategoryScoredCondition>
    {
        protected override bool Evaluate(GameState state, BuildingCategoryScoredCondition condition)
        {
            bool badEyesight = state.Boosters.Exists(b => b.Name == "Bad Eyesight");
            var buildingCategory = state.ScoringState.CurrentScoringBuilding.Building.Category;
            var conditionCategory = condition.Category;

            if (badEyesight)
            {
                if (conditionCategory == Category.Cat1)
                    conditionCategory = Category.Cat3;
                if (conditionCategory == Category.Cat2)
                    conditionCategory = Category.Cat4;

                if (buildingCategory == Category.Cat1)
                    buildingCategory = Category.Cat3;
                if (buildingCategory == Category.Cat2)
                    buildingCategory = Category.Cat4;
            }

            return buildingCategory == conditionCategory;
        }
    }
}