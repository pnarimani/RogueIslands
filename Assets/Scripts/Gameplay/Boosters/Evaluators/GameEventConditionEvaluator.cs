using System;
using System.Linq;
using RogueIslands.Gameplay.Boosters.Conditions;

namespace RogueIslands.Gameplay.Boosters.Evaluators
{
    public class GameEventConditionEvaluator : GameConditionEvaluator<IGameEventCondition>
    {
        public override bool CanHandle(Type conditionType) => typeof(IGameCondition).IsAssignableFrom(conditionType);

        protected override bool Evaluate(GameState state, IBooster booster, IGameEventCondition condition) 
            => condition.TriggeringEvents.Any(e => e.IsInstanceOfType(state.CurrentEvent));
    }
}