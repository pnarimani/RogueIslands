using System.Linq;
using RogueIslands.Boosters;
using RogueIslands.Boosters.Conditions;

namespace RogueIslands
{
    public class GameEventConditionEvaluator : GameConditionEvaluator<GameEventCondition>
    {
        protected override bool Evaluate(GameState state, IBooster booster, GameEventCondition condition) 
            => condition.TriggeringEvents.Any(e => e.IsInstanceOfType(state.CurrentEvent));
    }
}