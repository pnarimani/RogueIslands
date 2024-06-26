using System;
using RogueIslands.GameEvents;
using UnityEngine;

namespace RogueIslands.Buildings
{
    public static class BuildingPlacement
    {
        public static void PlaceBuilding(this GameState state, IGameView view, Building building, Vector3 position,
            Quaternion rotation)
        {
            building.Position = position;
            building.Rotation = rotation;

            view.SpawnBuilding(building);

            RemoveOverlappingWorldBoosters(state, view, building);

            PlaceBuildingInIsland(state, building);

            state.ExecuteEvent(view, new BuildingPlaced { Building = building });
        }

        private static void PlaceBuildingInIsland(GameState state, Building building)
        {
            if (state.TryGetClusterIdForBuilding(building, out var id))
            {
                building.ClusterId = id;
            }
            else
            {
                building.ClusterId = new ClusterId(Guid.NewGuid().GetHashCode());
            }
        }

        private static void RemoveOverlappingWorldBoosters(GameState state, IGameView view, Building building)
        {
            var bounds = view.GetBounds(building);
            for (var i = state.WorldBoosters.SpawnedBoosters.Count - 1; i >= 0; i--)
            {
                var wb = state.WorldBoosters.SpawnedBoosters[i];
                var wbBounds = view.GetBounds(wb);
                if (bounds.Intersects(wbBounds))
                {
                    state.WorldBoosters.SpawnedBoosters.RemoveAt(i);

                    state.ExecuteEvent(view, new WorldBoosterDestroyed { Booster = wb });

                    view.GetBooster(wb).Remove();
                }
            }
        }

        private static bool TryGetClusterIdForBuilding(this GameState state, Building building, out ClusterId id)
        {
            return TryGetClusterIdAtPosition(state, building.Position, building.Range, out id);
        }

        private static bool TryGetClusterIdAtPosition(this GameState state, Vector3 position, float range,
            out ClusterId id)
        {
            foreach (var other in state.PlacedDownBuildings)
            {
                var distance = Vector3.Distance(other.Position, position);
                var biggestRange = Math.Max(other.Range, range);
                if (distance < biggestRange)
                {
                    id = other.ClusterId;
                    return true;
                }
            }

            id = default;
            return false;
        }
    }
}