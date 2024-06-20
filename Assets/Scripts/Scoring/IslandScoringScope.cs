﻿using System;

namespace RogueIslands.Scoring
{
    public readonly struct IslandScoringScope : IDisposable
    {
        private readonly GameState _state;
        private readonly IGameView _view;
        private readonly Island _island;

        public IslandScoringScope(GameState state, IGameView view, Island island)
        {
            _island = island;
            _view = view;
            _state = state;

            state.ScoringState.SelectedIsland = island;
            state.ScoringState.SelectedBuilding = null;
            state.ExecuteEvent(view, "BeforeIslandScore");
            
            view.HighlightIsland(island);
        }

        public void Dispose()
        {
            _state.ScoringState.SelectedBuilding = null;
            _state.ExecuteEvent(_view, "AfterIslandScore");
            
            _view.LowlightIsland(_island);
        }
    }
}