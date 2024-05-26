using System;
using System.Linq;
using static RogueIslands.Boosters.CountCondition;

namespace RogueIslands.Boosters
{
    public class CountConditionEvaluator : ConditionEvaluator<CountCondition>
    {
        protected override bool Evaluate(GameState state, CountCondition condition)
        {
            if (condition.TargetType == Target.BuildingsInAnyIsland)
            {
                foreach (var island in state.PlacedBuildings)
                {
                    var count = island.Buildings.Count;
                    var meetsCondition = condition.ComparisonMode switch
                    {
                        Mode.Less => count < condition.Value,
                        Mode.More => count > condition.Value,
                        Mode.Equal => count == condition.Value,
                        Mode.Even => count % 2 == 0,
                        Mode.Odd => count % 2 == 1,
                        Mode.PowerOfTwo => (count & (count - 1)) == 0,
                        _ => false,
                    };

                    if (meetsCondition)
                    {
                        return true;
                    }
                }

                return false;
            }

            {
                var count = condition.TargetType switch
                {
                    Target.Buildings => state.PlacedBuildings.SelectMany(x => x.Buildings).Count(),
                    Target.Island => state.PlacedBuildings.Count,
                    Target.BuildingsInScoringIsland => state.ScoringState.CurrentScoringIsland.Buildings.Count,
                    Target.BuildingsInAnyIsland => throw new ArgumentOutOfRangeException(),
                    _ => throw new ArgumentOutOfRangeException(),
                };

                return condition.ComparisonMode switch
                {
                    Mode.Less => count < condition.Value,
                    Mode.More => count > condition.Value,
                    Mode.Equal => count == condition.Value,
                    Mode.Even => count % 2 == 0,
                    Mode.Odd => count % 2 == 1,
                    Mode.PowerOfTwo => (count & (count - 1)) == 0,
                    _ => false
                };
            }
        }
    }
}