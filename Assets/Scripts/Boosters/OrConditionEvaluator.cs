namespace RogueIslands.Boosters
{
    public class OrConditionEvaluator : ConditionEvaluator<OrCondition>
    {
        protected override bool Evaluate(GameState state, IBooster booster, OrCondition condition)
        {
            foreach (var subCondition in condition.Conditions)
            {
                if (state.IsConditionMet(booster, subCondition))
                    return true;
            }

            return false;
        }
    }
}