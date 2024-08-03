using System.Collections.Generic;

namespace RogueIslands.Gameplay.Boosters.Conditions
{
    public class OrConditions : IGameCondition
    {
        public OrConditions(IReadOnlyList<IGameCondition> conditions) => Conditions = conditions;
        public IReadOnlyList<IGameCondition> Conditions { get; set; }
    }
}