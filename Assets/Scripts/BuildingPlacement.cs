using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace RogueIslands
{
    public static class BuildingPlacement
    {
        public static void PlaceBuilding(this GameState state, IGameView view, Building buildingData, Vector3 position,
            Quaternion rotation)
        {
            Assert.IsTrue(buildingData.Id.IsDefault());

            var building = buildingData.Clone();
            building.Id = new BuildingInstanceId(Guid.NewGuid().GetHashCode());
            building.Position = position;
            building.Rotation = rotation;

            view.SpawnBuilding(building);

            RemoveOverlappingWorldBoosters(state, view, building);
            
            PlaceBuildingInIsland(state, building);

            state.BuildingsInHand.Remove(buildingData);

            state.ExecuteEvent(view, "BuildingPlaced");
        }

        private static void PlaceBuildingInIsland(GameState state, Building building)
        {
            if (state.GetIslands(building) is { Count: > 0 } islands)
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
                state.Islands.Add(new Island()
                {
                    Id = Guid.NewGuid().ToString(),
                    Buildings = new List<Building> { building },
                });
            }
        }

        private static void RemoveOverlappingWorldBoosters(GameState state, IGameView view, Building building)
        {
            var bounds = view.GetBounds(building);
            for (var i = state.WorldBoosters.Count - 1; i >= 0; i--)
            {
                var wb = state.WorldBoosters[i];
                var wbBounds = view.GetBounds(wb);
                if (bounds.Intersects(wbBounds))
                {
                    state.WorldBoosters.RemoveAt(i);
                    
                    state.ExecuteEvent(view, "BoosterRemoved");
                    state.ExecuteEvent(view, "WorldBoosterRemoved");
                    
                    view.GetBooster(wb).Remove();
                }
            }
        }

        private static void MergeIslands(GameState state, List<Island> islands, Building building)
        {
            var buildings = islands
                .SelectMany(island => island.Buildings)
                .Append(building);

            foreach (var island in islands)
                state.Islands.Remove(island);

            state.Islands.Add(new Island()
            {
                Id = Guid.NewGuid().ToString(),
                Buildings = buildings.ToList(),
            });
        }

        public static List<Island> GetIslands(this GameState state, Building building)
        {
            return GetIslands(state, building.Position, building.Range);
        }

        public static List<Island> GetIslands(this GameState state, Vector3 position, float range)
        {
            var islands = new List<Island>();
            foreach (var island in state.Islands)
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