using RogueIslands.GameEvents;

namespace RogueIslands.Boosters
{
    public class BuildingTriggerCountEvaluator : GameConditionEvaluator<BuildingTriggerCountCondition>
    {
        protected override bool Evaluate(GameState state, IBooster booster, BuildingTriggerCountCondition condition)
            => state.CurrentEvent is BuildingEvent e && e.TriggerCount == condition.TriggerCount;
    }
}