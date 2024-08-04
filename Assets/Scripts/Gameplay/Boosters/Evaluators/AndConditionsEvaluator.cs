using RogueIslands.Autofac;
using RogueIslands.Gameplay.Boosters.Conditions;

namespace RogueIslands.Gameplay.Boosters.Evaluators
{
    public class AndConditionsEvaluator : GameConditionEvaluator<AndConditions>
    {
        private GameConditionsController _conditionsController;

        public AndConditionsEvaluator(GameConditionsController conditionsController) 
            => _conditionsController = conditionsController;

        protected override bool Evaluate(GameState state, IBooster booster, AndConditions condition)
        {
            foreach (var subCondition in condition.Conditions)
            {
                if (!_conditionsController.IsConditionMet(booster, subCondition))
                    return false;
            }

            return true;
        }
    }
}