using System.Linq;

namespace RogueIslands.Boosters
{
    public class SelectedBuildingCategoryEvaluator : ConditionEvaluator<SelectedBuildingCategory>
    {
        protected override bool Evaluate(GameState state, IBooster booster, SelectedBuildingCategory condition) 
            => state.ScoringState is { SelectedBuilding: not null } && condition.Categories.Contains(state.ScoringState.SelectedBuilding.Category);
    }
}