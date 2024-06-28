using System;
using System.Collections.Generic;
using RogueIslands.Gameplay.GameEvents;
using UnityEngine;
using UnityEngine.Pool;

namespace RogueIslands.Gameplay.Buildings
{
    public class BuildingPlacement
    {
        private readonly GameState _state;
        private readonly IGameView _view;
        private readonly IEventController _eventController;

        public BuildingPlacement(GameState state, IGameView view, IEventController eventController)
        {
            _eventController = eventController;
            _view = view;
            _state = state;
        }

        public void PlaceBuilding(Building building, Vector3 position, Quaternion rotation)
        {
            building.Position = position;
            building.Rotation = rotation;

            _view.SpawnBuilding(building);

            RemoveOverlappingWorldBoosters(building);

            PlaceBuildingInIsland(building);

            _eventController.Execute(new BuildingPlaced { Building = building });
        }

        private void PlaceBuildingInIsland(Building building)
        {
            using var _ = ListPool<ClusterId>.Get(out var idList);
            foreach (var other in _state.PlacedDownBuildings)
            {
                if (idList.Contains(other.ClusterId))
                    continue;

                var distance = Vector3.Distance(other.Position, building.Position);
                var biggestRange = Math.Max(other.Range, building.Range);
                if (distance < biggestRange)
                    idList.Add(other.ClusterId);
            }

            if (idList.Count == 0)
            {
                building.ClusterId = ClusterId.NewClusterId();
                return;
            }

            if (idList.Count == 1)
            {
                building.ClusterId = idList[0];
                return;
            }

            MergeClusters(idList, building);
        }

        private void MergeClusters(List<ClusterId> existingClusters, Building newBuilding)
        {
            var newClusterId = ClusterId.NewClusterId();
            newBuilding.ClusterId = newClusterId;

            foreach (var building in _state.PlacedDownBuildings)
            {
                if(existingClusters.Contains(building.ClusterId))
                    building.ClusterId = newClusterId;
            }
        }

        private void RemoveOverlappingWorldBoosters(Building building)
        {
            var bounds = _view.GetBounds(building);
            for (var i = _state.WorldBoosters.SpawnedBoosters.Count - 1; i >= 0; i--)
            {
                var wb = _state.WorldBoosters.SpawnedBoosters[i];
                var wbBounds = _view.GetBounds(wb);
                if (bounds.Intersects(wbBounds))
                {
                    _state.WorldBoosters.SpawnedBoosters.RemoveAt(i);

                    _eventController.Execute(new WorldBoosterDestroyed { Booster = wb });

                    _view.GetBooster(wb).Remove();
                }
            }
        }
    }
}