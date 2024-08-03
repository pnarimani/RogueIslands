using System.Collections.Generic;

namespace RogueIslands.Gameplay.Boosters.Conditions
{
    public static class ConditionExtensions
    {
        public static AndConditions And(this IGameCondition cond, IGameCondition condition)
        {
            if (cond is AndConditions andConditions)
                return new AndConditions(new List<IGameCondition>(andConditions.Conditions) { condition });
            return new AndConditions(new List<IGameCondition> { cond, condition });
        }

        public static OrConditions Or(this IGameCondition cond, IGameCondition condition)
        {
            if (cond is OrConditions orConditions)
                return new OrConditions(new List<IGameCondition>(orConditions.Conditions) { condition });
            return new OrConditions(new List<IGameCondition> { cond, condition });
        }
    }
}