using System.Linq;
using RogueIslands.Gameplay.Boosters.Conditions;
using RogueIslands.Gameplay.GameEvents;

namespace RogueIslands.Gameplay.Boosters.Evaluators
{
    public class SelectedBuildingCategoryEvaluator : GameConditionEvaluator<BuildingCategoryCondition>
    {
        protected override bool Evaluate(GameState state, IBooster booster, BuildingCategoryCondition condition) 
            => state.CurrentEvent is BuildingEvent e && condition.Categories.Contains(e.Building.Category);
    }
}