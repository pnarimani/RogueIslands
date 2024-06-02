using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RogueIslands
{
    public static class BuildingPlacement
    {
        public static void PlaceBuilding(this GameState state, PlacedBuilding building)
        {
            state.Energy -= building.Building.EnergyCost;

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
                    Buildings = new List<PlacedBuilding> { building },
                });
            }

            ExecuteBuildingPlacedEvent(state);
        }

        private static void MergeIslands(GameState state, List<Island> islands, PlacedBuilding building)
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

        private static void ExecuteBuildingPlacedEvent(this GameState state)
        {
            state.CurrentEvent = "BuildingPlaced";
            state.ExecuteAll();
        }

        public static List<Island> GetIslands(this GameState state, PlacedBuilding building)
        {
            return GetIslands(state, building.Position, building.Building.Range);
        }

        public static List<Island> GetIslands(this GameState state, Vector3 position, float range)
        {
            var islands = new List<Island>();
            foreach (var island in state.Islands)
            {
                foreach (var other in island)
                {
                    var distance = Vector3.Distance(other.Position, position);
                    var biggestRange = Math.Max(other.Building.Range, range);
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