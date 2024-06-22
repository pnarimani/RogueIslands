using System.Collections.Generic;

namespace RogueIslands.Boosters
{
    public class OrCondition : IGameCondition
    {
        public IGameCondition[] Conditions { get; set; }

        public OrCondition(params IGameCondition[] conditions) => Conditions = conditions;
    }
}