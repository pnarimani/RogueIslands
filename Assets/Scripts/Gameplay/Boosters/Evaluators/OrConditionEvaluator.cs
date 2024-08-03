using RogueIslands.Autofac;
using RogueIslands.Gameplay.Boosters.Conditions;

namespace RogueIslands.Gameplay.Boosters.Evaluators
{
    public class OrConditionEvaluator : GameConditionEvaluator<OrConditions>
    {
        protected override bool Evaluate(GameState state, IBooster booster, OrConditions condition)
        {
            var m = StaticResolver.Resolve<GameConditionsController>();
            foreach (var subCondition in condition.Conditions)
            {
                if (m.IsConditionMet(booster, subCondition))
                    return true;
            }

            return false;
        }
    }
}