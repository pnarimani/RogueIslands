using System;
using System.Linq;
using RogueIslands.Gameplay.Boosters.Conditions;

namespace RogueIslands.Gameplay.Boosters.Evaluators
{
    public class GameEventConditionEvaluator : GameConditionEvaluator<IGameEventCondition>
    {
        public override bool CanHandle(Type conditionType) => typeof(IGameEventCondition).IsAssignableFrom(conditionType);

        protected override bool Evaluate(GameState state, IBooster booster, IGameEventCondition condition)
        {
            foreach (var e in condition.TriggeringEvents)
            {
                if (e.IsInstanceOfType(state.CurrentEvent))
                    return true;
            }

            return false;
        }
    }
}