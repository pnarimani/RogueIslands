using System;
using System.Linq;
using static RogueIslands.Boosters.CountCondition;

namespace RogueIslands.Boosters
{
    public class CountConditionEvaluator : GameConditionEvaluator<CountCondition>
    {
        protected override bool Evaluate(GameState state, IBooster booster, CountCondition condition)
        {
            if (condition.TargetType == Target.BuildingsInAnyIsland)
            {
                foreach (var island in state.Clusters)
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
                    Target.Buildings => state.Clusters.SelectMany(x => x.Buildings).Count(),
                    Target.Cluster => state.Clusters.Count,
                    Target.BuildingsInScoringIsland => throw new NotImplementedException(),
                    Target.BuildingsInAnyIsland => throw new NotImplementedException(),
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