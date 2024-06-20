using System;

namespace RogueIslands.Scoring
{
    public readonly struct BuildingScoringScope : IDisposable
    {
        private readonly GameState _state;
        private readonly IGameView _view;

        public BuildingScoringScope(GameState state, IGameView view, Building building)
        {
            _view = view;
            _state = state;

            state.ScoringState.SelectedBuilding = building;
            state.ScoringState.SelectedBuildingTriggerCount = 0;
        }

        public void TriggerBeforeScore()
        {
            _state.ExecuteEvent(_view, "BeforeBuildingScored");
        }

        public void TriggerAfterScore()
        {
            _state.ScoringState.SelectedBuildingTriggerCount++;
            _state.ExecuteEvent(_view, "AfterBuildingScored");
        }

        public void BuildingRemainedInHand()
        {
            _state.ScoringState.SelectedBuildingTriggerCount++;
            _state.ExecuteEvent(_view, "BuildingRemainedInHand");
        }

        public void Dispose()
        {
            _state.ScoringState.SelectedBuilding = null;
            _state.ScoringState.SelectedBuildingTriggerCount = 0;
        }
    }
}