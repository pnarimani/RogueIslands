using RogueIslands.Autofac;
using RogueIslands.Gameplay.Boosters.Conditions;

namespace RogueIslands.Gameplay.Boosters.Evaluators
{
    public class OrConditionEvaluator : GameConditionEvaluator<OrConditions>
    {
        private GameConditionsController _conditionsController;

        public OrConditionEvaluator(GameConditionsController conditionsController)
        {
            _conditionsController = conditionsController;
        }
        
        protected override bool Evaluate(GameState state, IBooster booster, OrConditions condition)
        {
            foreach (var subCondition in condition.Conditions)
            {
                if (_conditionsController.IsConditionMet(booster, subCondition))
                    return true;
            }

            return false;
        }
    }
}