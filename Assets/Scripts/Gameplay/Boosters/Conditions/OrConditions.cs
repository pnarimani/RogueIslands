using System.Collections.Generic;

namespace RogueIslands.Gameplay.Boosters.Conditions
{
    public class OrConditions : IGameCondition
    {
        public IReadOnlyList<IGameCondition> Conditions { get; set; }

        public OrConditions(IReadOnlyList<IGameCondition> conditions)
        {
            Conditions = conditions;
        }

        public bool Evaluate(IBooster booster)
        {
            foreach (var subCondition in Conditions)
                if (subCondition.Evaluate(booster))
                    return true;

            return false;
        }
    }
}