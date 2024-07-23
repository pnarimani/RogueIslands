﻿using System.Linq;
using RogueIslands.Serialization;
using UnityEngine;

namespace RogueIslands.Gameplay.Buildings
{
    public class BuildingPlacement
    {
        private readonly ICloner _cloner;
        private readonly IEventController _eventController;
        private readonly ScoringController _scoringController;
        private readonly GameState _state;
        private readonly IGameView _view;
        private RoundController _roundController;

        public BuildingPlacement(GameState state, IGameView view, IEventController eventController, ICloner cloner,
            ScoringController scoringController, RoundController roundController)
        {
            _roundController = roundController;
            _scoringController = scoringController;
            _cloner = cloner;
            _eventController = eventController;
            _view = view;
            _state = state;
        }

        public void PlaceBuilding(Building building, Vector3 position, Quaternion rotation)
        {
            building.Position = position;
            building.Rotation = rotation;
            
            _state.Buildings.Deck.Remove(building);
            _state.Buildings.PlacedDownBuildings.Add(building);

            _view.SpawnBuilding(building);

            _scoringController.ScoreBuilding(building);

            _view.GetUI().RemoveCard(building);
        }
    }
}