using System.Linq;
using RogueIslands.GameEvents;

namespace RogueIslands.Boosters
{
    public class SelectedBuildingCategoryEvaluator : ConditionEvaluator<SelectedBuildingCategory>
    {
        protected override bool Evaluate(GameState state, IBooster booster, SelectedBuildingCategory condition) 
            => state.CurrentEvent is BuildingEvent e && condition.Categories.Contains(e.Building.Category);
    }
}