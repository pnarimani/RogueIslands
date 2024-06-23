using System.Collections.Generic;
using System.Linq;
using RogueIslands.Boosters;
using UnityEngine.Assertions;

namespace RogueIslands
{
    public class GameConditionsController
    {
        private readonly GameState _state;
        private IReadOnlyList<GameConditionEvaluator> _evals;

        public GameConditionsController(GameState state)
        {
            _state = state;
        }

        public void SetEvaluators(IReadOnlyList<GameConditionEvaluator> evals)
        {
            _evals = evals;
        }

        public bool IsConditionMet(IBooster booster, IGameCondition condition)
        {
            var evaluator = _evals.First(x => x.ConditionType == condition.GetType());
            Assert.IsNotNull(evaluator, $"No evaluator found for condition type {condition.GetType().Name}");
            return evaluator.Evaluate(_state, booster, condition);
        }
    }
}