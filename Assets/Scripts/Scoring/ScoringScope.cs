﻿using System;

namespace RogueIslands.Scoring
{
    public readonly struct ScoringScope : IDisposable
    {
        private readonly GameState _state;
        private readonly IGameView _view;

        public ScoringScope(GameState state, IGameView view)
        {
            _view = view;
            _state = state;
            state.ScoringState = new ScoringState();
            state.ExecuteEvent(view, "DayStart");
        }

        public void Dispose()
        {
            _state.ScoringState.SelectedIsland = null;
            _state.ScoringState.SelectedBuilding = null;
            _state.ExecuteEvent(_view, "DayEnd");
        }
    }
}