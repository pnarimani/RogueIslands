using System;

namespace RogueIslands.Scoring
{
    public struct BuildingScoringScope : IDisposable
    {
        private readonly GameState _state;
        private readonly IGameView _view;
        private bool _hasTriggeredOnce;

        public BuildingScoringScope(GameState state, IGameView view, Building building)
        {
            _view = view;
            _state = state;
            _hasTriggeredOnce = false;

            state.ScoringState.SelectedBuilding = building;
        }

        public void TriggerBeforeScore()
        {
            _state.ExecuteEvent(_view, "BeforeBuildingScored");
        }

        public void TriggerAfterScore()
        {
            if (!_hasTriggeredOnce)
                _state.ExecuteEvent(_view, "BuildingFirstTrigger");
            _hasTriggeredOnce = true;
            _state.ExecuteEvent(_view, "AfterBuildingScored");
        }

        public void BuildingRemainedInHand()
        {
            _state.ExecuteEvent(_view, "BuildingRemainedInHand");
        }

        public void Dispose()
        {
            _state.ScoringState.SelectedBuilding = null;
        }
    }
}