using RogueIslands.Autofac;
using RogueIslands.Gameplay.Boosters.Conditions;

namespace RogueIslands.Gameplay.Boosters.Evaluators
{
    public class AndConditionsEvaluator : GameConditionEvaluator<AndConditions>
    {
        protected override bool Evaluate(GameState state, IBooster booster, AndConditions condition)
        {
            var m = StaticResolver.Resolve<GameConditionsController>();
            foreach (var subCondition in condition.Conditions)
            {
                if (!m.IsConditionMet(booster, subCondition))
                    return false;
            }

            return true;
        }
    }
}