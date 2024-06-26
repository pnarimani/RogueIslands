using System;
using System.Collections.Generic;
using System.Linq;
using RogueIslands.GameEvents;
using RogueIslands.Serialization;
using UnityEngine;
using UnityEngine.Assertions;

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
            if (state.GetClusterForBuilding(building) is { Count: > 0 } islands)
            {
                if (islands.Count == 1)
                {
                    islands[0].Buildings.Add(building);
                }
                else
                {
                    MergeIslands(state, islands, building);
                }
            }
            else
            {
                state.Clusters.Add(new Cluster()
                {
                    Id = Guid.NewGuid().ToString(),
                    Buildings = new List<Building> { building },
                });
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

        private static void MergeIslands(GameState state, List<Cluster> islands, Building building)
        {
            var buildings = islands
                .SelectMany(island => island.Buildings)
                .Append(building);

            foreach (var island in islands)
                state.Clusters.Remove(island);

            state.Clusters.Add(new Cluster()
            {
                Id = Guid.NewGuid().ToString(),
                Buildings = buildings.ToList(),
            });
        }

        public static List<Cluster> GetClusterForBuilding(this GameState state, Building building)
        {
            return GetClusterAtPosition(state, building.Position, building.Range);
        }

        public static List<Cluster> GetClusterAtPosition(this GameState state, Vector3 position, float range)
        {
            var islands = new List<Cluster>();
            foreach (var island in state.Clusters)
            {
                foreach (var other in island)
                {
                    var distance = Vector3.Distance(other.Position, position);
                    var biggestRange = Math.Max(other.Range, range);
                    if (distance < biggestRange)
                    {
                        islands.Add(island);
                        break;
                    }
                }
            }

            return islands;
        }
    }
}