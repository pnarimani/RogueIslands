using System.Linq;
using RogueIslands.Gameplay.Boosters.Conditions;
using RogueIslands.Gameplay.Boosters.Sources;
using RogueIslands.Gameplay.GameEvents;

namespace RogueIslands.Gameplay.Boosters.Evaluators
{
    public class BuildingCategoryEvaluator : GameConditionEvaluator<BuildingCategoryCondition>
    {
        protected override bool Evaluate(GameState state, IBooster booster, BuildingCategoryCondition condition)
        {
            condition.Source ??= new BuildingFromCurrentEvent();
            
            foreach (var building in condition.Source.Get(state, booster))
            {
                if (!condition.Categories.Contains(building.Category))
                    return false;
            }

            return true;
        }
    }
}