using System;
using System.Linq;
using RogueIslands.Gameplay.Boosters.Conditions;
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
            if (condition.TargetType == Target.BuildingsInAnyIsland)
                return false;

            if (condition.TargetType == Target.BuildingsInScoringIsland &&
                state.CurrentEvent is not BuildingPlaced)
                return false;

            {
                var count = condition.TargetType switch
                {
                    Target.Buildings => state.PlacedDownBuildings.Count(),
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
            if (state.CurrentEvent is BuildingPlaced buildingScored)
                return state.PlacedDownBuildings.Count(b => Vector3.Distance(b.Position, buildingScored.Building.Position) <= buildingScored.Building.Range);
            throw new ArgumentException("Current event is not a ClusterScored or BuildingScored event.");
        }
    }
}