using System.Linq;
using RogueIslands.Buildings;
using RogueIslands.GameEvents;
using UnityEngine.Pool;

namespace RogueIslands
{
    public class PlayController
    {
        private readonly IGameView _view;
        private readonly GameState _state;
        private readonly EventController _eventController;

        public PlayController(IGameView view, GameState state, EventController eventController)
        {
            _eventController = eventController;
            _state = state;
            _view = view;
        }

        public void Play()
        {
            foreach (var building in _state.Buildings.Deck)
                building.RemainingTriggers = 1;

            _state.ScoringState = new ScoringState();
            _eventController.Execute(new DayStart());

            TriggerPlacedBuildings();
            TriggerBuildingsInHand();

            _eventController.Execute(new DayEnd());

            _state.CurrentScore += _state.ScoringState.Products * _state.ScoringState.Multiplier;
            _state.Day++;
            _state.ScoringState = null;

            _view.GetUI().RefreshDate();

            _state.Validate();
        }

        private void TriggerPlacedBuildings()
        {
            foreach (var cluster in _state.GetClusters())
            {
                foreach (var building in cluster)
                {
                    BuildingScored buildingEvent = new()
                    {
                        Cluster = cluster,
                        Building = building,
                    };

                    var buildingView = _view.GetBuilding(building);

                    while (building.RemainingTriggers > 0)
                        TriggerBuilding(building, buildingView, buildingEvent);
                }
            }
        }

        private void TriggerBuilding(Building building, IBuildingView buildingView, BuildingScored buildingScored)
        {
            building.RemainingTriggers--;
            _state.ScoringState.Products += building.Output + building.OutputUpgrade;
            buildingView.BuildingTriggered(false);
            buildingScored.TriggerCount++;
            _eventController.Execute(buildingScored);
        }

        public void ProcessScore()
        {
            _state.ProcessScore(_view);
        }

        private void TriggerBuildingsInHand()
        {
            BuildingRemainedInHand buildingRemainedInHand = new();

            foreach (var building in _state.BuildingsInHand)
            {
                if (building.IsPlacedDown())
                    continue;
                buildingRemainedInHand.Building = building;
                buildingRemainedInHand.TriggerCount = 1;
                _eventController.Execute(buildingRemainedInHand);
            }
        }
    }
}