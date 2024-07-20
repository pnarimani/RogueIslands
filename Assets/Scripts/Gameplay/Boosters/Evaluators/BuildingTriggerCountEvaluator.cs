using RogueIslands.Gameplay.Boosters.Conditions;
using RogueIslands.Gameplay.GameEvents;

namespace RogueIslands.Gameplay.Boosters.Evaluators
{
    public class BuildingTriggerCountEvaluator : GameConditionEvaluator<BuildingTriggerCountCondition>
    {
        protected override bool Evaluate(GameState state, IBooster booster, BuildingTriggerCountCondition condition)
            => state.CurrentEvent is BuildingTriggered e && e.TriggerCount == condition.TriggerCount;
    }
}