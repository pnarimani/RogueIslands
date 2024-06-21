using System.Linq;

namespace RogueIslands.Boosters
{
    public class SelectedBuildingColorEvaluator : ConditionEvaluator<SelectedBuildingColorCondition>
    {
        protected override bool Evaluate(GameState state, IBooster booster, SelectedBuildingColorCondition condition) 
            => state.ScoringState.SelectedBuilding is { } building && condition.Colors.Contains(building.Color);
    }
}