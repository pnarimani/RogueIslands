using RogueIslands.Gameplay.Boosters.Conditions;
using RogueIslands.Gameplay.GameEvents;

namespace RogueIslands.Gameplay.Boosters.Evaluators
{
    public class BuildingTriggerCountEvaluator : GameConditionEvaluator<BuildingTriggerCountCondition>
    {
        protected override bool Evaluate(GameState state, IBooster booster, BuildingTriggerCountCondition condition)
        {
            var ev = state.CurrentEvent;
            return (ev is BuildingTriggered trigger && trigger.TriggerCount == condition.TriggerCount) ||
                   (ev is AfterBuildingScoreTrigger after && after.TriggerCount == condition.TriggerCount);
        }
    }
}