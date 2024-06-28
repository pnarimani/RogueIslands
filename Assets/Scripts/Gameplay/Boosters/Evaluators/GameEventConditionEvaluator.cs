using System.Linq;
using RogueIslands.Gameplay.Boosters.Conditions;

namespace RogueIslands.Gameplay.Boosters.Evaluators
{
    public class GameEventConditionEvaluator : GameConditionEvaluator<GameEventCondition>
    {
        protected override bool Evaluate(GameState state, IBooster booster, GameEventCondition condition) 
            => condition.TriggeringEvents.Any(e => e.IsInstanceOfType(state.CurrentEvent));
    }
}