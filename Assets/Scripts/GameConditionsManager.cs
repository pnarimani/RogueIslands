using System.Collections.Generic;
using System.Linq;
using RogueIslands.Boosters;
using UnityEngine.Assertions;

namespace RogueIslands
{
    public static class GameConditionsManager
    {
        private static IReadOnlyList<GameConditionEvaluator> _defaultEvaluators;

        public static bool IsConditionMet(this GameState state, IBooster booster, IGameCondition condition)
        {
            Assert.IsNotNull(condition);

            _defaultEvaluators ??= StaticResolver.Resolve<IReadOnlyList<GameConditionEvaluator>>();

            Assert.IsNotNull(_defaultEvaluators);

            var evaluator = _defaultEvaluators.First(x => x.ConditionType == condition.GetType());

            Assert.IsNotNull(evaluator);

            return evaluator.Evaluate(state, booster, condition);
        }
    }
}