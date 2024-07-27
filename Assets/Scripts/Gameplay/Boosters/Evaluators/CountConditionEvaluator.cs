using System;
using System.Linq;
using RogueIslands.Gameplay.Boosters.Conditions;
using RogueIslands.Gameplay.Boosters.Sources;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.GameEvents;
using UnityEngine;
using static RogueIslands.Gameplay.Boosters.Conditions.CountCondition;

namespace RogueIslands.Gameplay.Boosters.Evaluators
{
    public class CountConditionEvaluator : GameConditionEvaluator<CountCondition>
    {
        protected override bool Evaluate(GameState state, IBooster booster, CountCondition condition)
        {
            if (condition.Source == null)
                throw new InvalidOperationException($"Source is null. Booster: {booster.Name}");
            
            var count = condition.Source.Get(state, booster).Count();

            return condition.ComparisonMode switch
            {
                Mode.Less => count < condition.Value,
                Mode.More => count > condition.Value,
                Mode.Equal => count == condition.Value,
                Mode.Even => count % 2 == 0,
                Mode.Odd => count % 2 == 1,
                Mode.PowerOfTwo => (count & (count - 1)) == 0,
                _ => false,
            };
        }
    }
}