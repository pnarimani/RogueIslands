using System.Linq;
using RogueIslands.Gameplay.Boosters.Conditions;

namespace RogueIslands.Gameplay.Boosters.Actions
{
    public static class GameActionExtensions
    {
        public static CompositeAction And(this GameAction main, GameAction extra)
        {
            if (main is CompositeAction composite)
                return new CompositeAction
                {
                    Actions = composite.Actions.Append(extra).ToList(),
                };

            return new CompositeAction
            {
                Actions = new[] { main, extra },
            };
        }
        
        public static T When<T>(this T action, IGameCondition condition) where T : GameAction
        {
            action.Condition = condition;
            return action;
        }
    }
}