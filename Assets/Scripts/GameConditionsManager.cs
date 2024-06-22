using System.Collections.Generic;
using System.Linq;
using RogueIslands.Boosters;
using UnityEngine.Assertions;

namespace RogueIslands
{
    public static class GameConditionsManager
    {
        private static IReadOnlyList<GameConditionEvaluator> _defaultEvaluators;
        private static readonly List<GameConditionEvaluator> _evaluatorOverrides = new();
        
        public static bool IsConditionMet(this GameState state, IBooster booster, IGameCondition condition)
        {
            Assert.IsNotNull(condition);

            _defaultEvaluators ??= StaticResolver.Resolve<IReadOnlyList<GameConditionEvaluator>>();

            Assert.IsNotNull(_evaluatorOverrides);
            Assert.IsNotNull(_defaultEvaluators);

            var evaluator = _evaluatorOverrides.FirstOrDefault(x => x.ConditionType == condition.GetType()) ??
                            _defaultEvaluators.First(x => x.ConditionType == condition.GetType());

            Assert.IsNotNull(evaluator);

            return evaluator.Evaluate(state, booster, condition);
        }
        
        public static void RegisterEvaluatorOverride(GameConditionEvaluator evaluator)
        {
            _evaluatorOverrides.Add(evaluator);
        }
        
        public static void UnregisterEvaluatorOverride(GameConditionEvaluator evaluator)
        {
            _evaluatorOverrides.Remove(evaluator);
        }
    }
}