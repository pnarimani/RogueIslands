using RogueIslands.GameEvents;

namespace RogueIslands.Boosters
{
    public class BuildingTriggerCountEvaluator : ConditionEvaluator<BuildingTriggerCountCheck>
    {
        protected override bool Evaluate(GameState state, IBooster booster, BuildingTriggerCountCheck condition)
            => state.CurrentEvent is BuildingEvent e && e.TriggerCount == condition.TriggerCount;
    }
}