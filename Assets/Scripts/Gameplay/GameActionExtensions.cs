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
            
            if (action.Conditions != null)
            {
                foreach (var c in action.Conditions)
                {
                    if (c is OrCondition or)
                    {
                        result.AddRange(or.Conditions);
                    }
                    else
                    {
                        result.Add(c);
                    }
                }
            }

            if (action is CompositeAction composite)
            {
                foreach (var innerAction in composite.Actions)
                {
                    using var innerConditions = GetAllConditions(innerAction);
                    result.AddRange(innerConditions);
                }
            }

            return result;
        }
    }
}