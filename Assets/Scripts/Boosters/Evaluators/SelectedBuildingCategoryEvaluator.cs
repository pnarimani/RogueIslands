using System.Linq;
using RogueIslands.GameEvents;

namespace RogueIslands.Boosters
{
    public class SelectedBuildingCategoryEvaluator : GameConditionEvaluator<BuildingCategoryCondition>
    {
        protected override bool Evaluate(GameState state, IBooster booster, BuildingCategoryCondition condition) 
            => state.CurrentEvent is BuildingEvent e && condition.Categories.Contains(e.Building.Category);
    }
}