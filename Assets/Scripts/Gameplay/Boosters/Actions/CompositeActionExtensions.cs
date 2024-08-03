using System.Linq;

namespace RogueIslands.Gameplay.Boosters.Actions
{
    public static class CompositeActionExtensions
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
    }
}