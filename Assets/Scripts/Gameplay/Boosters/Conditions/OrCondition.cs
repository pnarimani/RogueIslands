using System.Collections.Generic;

namespace RogueIslands.Gameplay.Boosters.Conditions
{
    public class OrCondition : IGameCondition
    {
        public IReadOnlyList<IGameCondition> Conditions { get; set; }

        public OrCondition(params IGameCondition[] conditions) => Conditions = conditions;
    }
}