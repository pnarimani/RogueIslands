using RogueIslands.Gameplay.Boosters.Conditions;

namespace RogueIslands.Gameplay.Boosters.Evaluators
{
    public class OrConditionEvaluator : GameConditionEvaluator<OrCondition>
    {
        protected override bool Evaluate(GameState state, IBooster booster, OrCondition condition)
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