using System.Collections.Generic;

namespace RogueIslands.Gameplay.Boosters.Conditions
{
    public class AndConditions : IGameCondition
    {
        public IReadOnlyList<IGameCondition> Conditions { get; set; }

        public AndConditions(IReadOnlyList<IGameCondition> conditions) => Conditions = conditions;
    }
}