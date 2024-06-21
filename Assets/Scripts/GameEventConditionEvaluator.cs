using System.Linq;
using RogueIslands.Boosters;

namespace RogueIslands
{
    public class GameEventConditionEvaluator : ConditionEvaluator<GameEventCondition>
    {
        protected override bool Evaluate(GameState state, IBooster booster, GameEventCondition condition) 
            => condition.TriggeringEvents.Any(triggeringEvent => state.CurrentEvent == triggeringEvent);
    }
}