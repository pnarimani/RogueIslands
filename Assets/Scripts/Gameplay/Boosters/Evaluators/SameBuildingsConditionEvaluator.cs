using System.Linq;
using RogueIslands.Gameplay.Boosters.Conditions;

namespace RogueIslands.Gameplay.Boosters.Evaluators
{
    public class SameBuildingsConditionEvaluator : GameConditionEvaluator<SameBuildingsCondition>
    {
        protected override bool Evaluate(GameState state, IBooster booster, SameBuildingsCondition condition)
        {
            var buildings = state.PlacedDownBuildings.ToList();
            return buildings.Count <= 1 || buildings.All(other =>
            {
                var first = buildings[0];
                return first.Color == other.Color && first.Category == other.Category && first.Size == other.Size;
            });
        }
    }
} 