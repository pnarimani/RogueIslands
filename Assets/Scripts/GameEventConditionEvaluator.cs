using System.Linq;
using RogueIslands.Boosters;

namespace RogueIslands
{
    public class GameEventConditionEvaluator : ConditionEvaluator<GameEventCondition>
    {
        protected override bool Evaluate(GameState state, GameEventCondition condition) 
            => condition.TriggeringEvents.Any(triggeringEvent => state.CurrentEvent == triggeringEvent);
    }
}