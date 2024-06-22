using System.Collections.Generic;
using RogueIslands.Boosters;
using RogueIslands.Boosters.Actions;

namespace RogueIslands
{
    public static class GameActionExtensions
    {
        public static List<IGameCondition> GetAllConditions(this GameAction action)
        {
            var result = new List<IGameCondition>();
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

            if (action is CompositeAction composite)
            {
                foreach (var innerAction in composite.Actions)
                {
                    result.AddRange(GetAllConditions(innerAction));
                }
            }

            return result;
        }
    }
}