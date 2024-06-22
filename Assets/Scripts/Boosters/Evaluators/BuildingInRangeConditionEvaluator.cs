using RogueIslands.GameEvents;

namespace RogueIslands.Boosters
{
    public class BuildingInRangeConditionEvaluator : GameConditionEvaluator<BuildingInRangeCondition>
    {
        protected override bool Evaluate(GameState state, IBooster booster, BuildingInRangeCondition condition)
        {
            if (booster is not WorldBooster worldBooster)
                return false;

            if (state.CurrentEvent is not BuildingEvent { Building: { } building })
                return false;

            var sqrDist = (building.Position - worldBooster.Position).sqrMagnitude;
            return sqrDist <= worldBooster.Range * worldBooster.Range;
        }
    }
}