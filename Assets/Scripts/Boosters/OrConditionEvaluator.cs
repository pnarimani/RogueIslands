namespace RogueIslands.Boosters
{
    public class OrConditionEvaluator : ConditionEvaluator<OrCondition>
    {
        protected override bool Evaluate(GameState state, OrCondition condition)
        {
            foreach (var subCondition in condition.Conditions)
            {
                if (state.IsConditionMet(subCondition))
                    return true;
            }

            return false;
        }
    }
}