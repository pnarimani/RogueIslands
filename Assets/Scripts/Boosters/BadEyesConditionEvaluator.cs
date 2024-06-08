﻿namespace RogueIslands.Boosters
{
    public class BadEyesConditionEvaluator : ConditionEvaluator<BuildingCategoryScoredCondition>, IEvaluationConditionOverride
    {
        protected override bool Evaluate(GameState state, BuildingCategoryScoredCondition condition)
        {
            if (state.ScoringState is not { CurrentScoringBuilding: not null })
                return false;
            
            var buildingCategory = state.ScoringState.CurrentScoringBuilding.Category;
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