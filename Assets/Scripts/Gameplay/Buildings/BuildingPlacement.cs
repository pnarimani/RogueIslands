using System.Linq;
using RogueIslands.Diagnostics;
using UnityEngine;

namespace RogueIslands.Gameplay.Buildings
{
    public class BuildingPlacement
    {
        private readonly ScoringController _scoringController;
        private readonly GameState _state;
        private readonly IGameView _view;

        public BuildingPlacement(GameState state, IGameView view, ScoringController scoringController)
        {
            _scoringController = scoringController;
            _view = view;
            _state = state;
        }

        public void PlaceBuilding(Building building, Vector3 position, Quaternion rotation)
        {
            using var profiler = ProfilerBlock.Begin();

            building.Position = position;
            building.Rotation = rotation;

            var previousPeek = _state.DeckPeek.ToList();
            var previousHand = _state.BuildingsInHand.ToList();

            _state.Buildings.Deck.Remove(building);
            _state.Buildings.PlacedDownBuildings.Add(building);

            _view.SpawnBuilding(building);

            _view.GetUI().RemoveCard(building);
            _view.GetUI().RefreshDeckText();

            foreach (var handBuilding in _state.BuildingsInHand.Where(c => !previousHand.Contains(c)))
                _view.GetUI().MoveCardToHand(handBuilding);

            foreach (var peekBuilding in _state.DeckPeek.Where(c => !previousPeek.Contains(c)))
                _view.GetUI().ShowBuildingCardPeek(peekBuilding);

            _scoringController.ScoreBuilding(building);
        }
    }
}