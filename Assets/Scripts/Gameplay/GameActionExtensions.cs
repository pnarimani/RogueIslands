using RogueIslands.Gameplay.Boosters.Actions;
using RogueIslands.Gameplay.Boosters.Conditions;

namespace RogueIslands.Gameplay
{
    public static class GameActionExtensions
    {
        public static PooledList<IGameCondition> GetAllConditions(this GameAction action)
        {
            var result = PooledList<IGameCondition>.CreatePooled();
            if (action == null)
                return result;

            if (action.Condition != null)
            {
                if (action.Condition is OrConditions or)
                    result.AddRange(or.Conditions);
                else if (action.Condition is AndConditions and)
                    result.AddRange(and.Conditions);
                else
                    result.Add(action.Condition);
            }

            if (action is CompositeAction composite)
                foreach (var innerAction in composite.Actions)
                {
                    using var innerConditions = GetAllConditions(innerAction);
                    result.AddRange(innerConditions);
                }

            return result;
        }
    }
}