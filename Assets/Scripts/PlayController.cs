using System.Linq;
using RogueIslands.Buildings;
using RogueIslands.GameEvents;

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
            foreach (var building in _state.Clusters.SelectMany(island => island.Buildings))
                building.RemainingTriggers = 1;

            _state.ScoringState = new ScoringState();
            _eventController.Execute(new DayStart());

            foreach (var cluster in _state.Clusters) 
                ProcessCluster(cluster);
            
            TriggerBuildingsInHand();
            
            _eventController.Execute(new DayEnd());

            _state.CurrentScore += _state.ScoringState.Products * _state.ScoringState.Multiplier;
            _state.Day++;
            _state.ScoringState = null;

            _view.GetUI().RefreshDate();

            _state.Validate();
        }

        private void ProcessCluster(Cluster cluster)
        {
            foreach (var building in cluster.Buildings)
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

            _eventController.Execute(new ClusterScored { Cluster = cluster });
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
                buildingRemainedInHand.Building = building;
                buildingRemainedInHand.TriggerCount = 1;
                _eventController.Execute(buildingRemainedInHand);
            }
        }
    }
}