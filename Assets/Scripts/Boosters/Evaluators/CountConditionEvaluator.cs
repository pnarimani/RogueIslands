using System;
using System.Linq;
using RogueIslands.GameEvents;
using static RogueIslands.Boosters.CountCondition;

namespace RogueIslands.Boosters.Evaluators
{
    public class CountConditionEvaluator : GameConditionEvaluator<CountCondition>
    {
        protected override bool Evaluate(GameState state, IBooster booster, CountCondition condition)
        {
            if (condition.TargetType == Target.BuildingsInAnyIsland)
                return EvaluateBuildingsInAnyIsland(state, condition);

            if (condition.TargetType == Target.BuildingsInScoringIsland &&
                state.CurrentEvent is not BuildingScored or ClusterScored)
                return false;

            {
                var count = condition.TargetType switch
                {
                    Target.Buildings => state.Clusters.SelectMany(x => x.Buildings).Count(),
                    Target.Cluster => state.Clusters.Count,
                    Target.BuildingsInScoringIsland => GetBuildingsInScoringCluster(state),
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
                    _ => false,
                };
            }
        }

        private static int GetBuildingsInScoringCluster(GameState state)
        {
            if (state.CurrentEvent is ClusterScored clusterScored)
                return clusterScored.Cluster.Buildings.Count;
            if (state.CurrentEvent is BuildingScored buildingScored)
                return buildingScored.Cluster.Buildings.Count;
            throw new ArgumentException("Current event is not a ClusterScored or BuildingScored event.");
        }

        private static bool EvaluateBuildingsInAnyIsland(GameState state, CountCondition condition)
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
    }
}