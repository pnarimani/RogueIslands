using System;
using System.Collections.Generic;
using System.Linq;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.Boosters.Conditions;
using RogueIslands.Gameplay.Boosters.Evaluators;
using UnityEngine.Assertions;

namespace RogueIslands.Gameplay
{
    public class GameConditionsController
    {
        private readonly GameState _state;
        private readonly Lazy<IReadOnlyList<GameConditionEvaluator>> _evaluators;

        public GameConditionsController(GameState state, Lazy<IReadOnlyList<GameConditionEvaluator>> evaluators)
        {
            _state = state;
            _evaluators = evaluators;
        }

        public bool IsConditionMet(IBooster booster, IGameCondition condition)
        {
            var conditionType = condition.GetType();
            var evaluator = _evaluators.Value.FirstOrDefault(x => x.CanHandle(conditionType));
            Assert.IsNotNull(evaluator, $"No evaluator found for condition type {conditionType.Name}");
            return evaluator.Evaluate(_state, booster, condition);
        }
    }
}