using System.Linq;

namespace RogueIslands.Boosters
{
    public class SameBuildingsConditionEvaluator : ConditionEvaluator<SameBuildingsCondition>
    {
        protected override bool Evaluate(GameState state, SameBuildingsCondition condition)
        {
            var buildings = state.Islands.SelectMany(x => x).ToList();
            return buildings.Count <= 1 || buildings.All(other =>
            {
                var first = buildings[0];
                return first.Color == other.Color && first.Category == other.Category && first.Size == other.Size;
            });
        }
    }
}