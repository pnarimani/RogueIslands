using System;
using System.Collections.Generic;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.DryRun
{
    public class DryRunGameView : IGameView
    {
        private readonly IGameView _realGameView;
        private Dictionary<BuildingId, DryRunBuildingView> _lastFrameBuildingViews = new();
        private Dictionary<BoosterInstanceId, DryRunBoosterView> _lastFrameBoosterViews = new();

        private Dictionary<BuildingId, DryRunBuildingView> _allProbabilitiesBuildingViews = new();
        private Dictionary<BoosterInstanceId, DryRunBoosterView> _allProbabilitiesBoosterViews = new();

        private Dictionary<BuildingId, DryRunBuildingView> _noProbabilitiesBuildingViews = new();
        private Dictionary<BoosterInstanceId, DryRunBoosterView> _noProbabilitiesBoosterViews = new();
        
        private readonly GameState _state;

        public DryRunGameView(IGameView realGameView, GameState state)
        {
            _state = state;
            _realGameView = realGameView;
        }

        public bool IsAllProbabilitiesMode { get; set; }

        public IBuildingView GetBuilding(Building building)
        {
            var dict = IsAllProbabilitiesMode ? _allProbabilitiesBuildingViews : _noProbabilitiesBuildingViews;
            if (!dict.TryGetValue(building.Id, out var view))
            {
                view = new DryRunBuildingView(_realGameView.GetBuilding(building));
                dict.Add(building.Id, view);
            }

            return view;
        }

        public IBoosterView GetBooster(BoosterInstanceId boosterId)
        {
            var dict = IsAllProbabilitiesMode ? _allProbabilitiesBoosterViews : _noProbabilitiesBoosterViews;
            if (!dict.TryGetValue(boosterId, out var view))
            {
                view = new DryRunBoosterView();
                dict.Add(boosterId, view);
            }

            return view;
        }

        public void ShowLoseScreen()
        {
        }

        public void ShowGameWinScreen()
        {
        }

        public IRoundWinScreen ShowRoundWin() => throw new NotImplementedException();

        public void AddBooster(IBooster instance)
        {
        }

        public IGameUI GetUI() => throw new NotImplementedException();

        public void SpawnBuilding(Building building) => throw new NotImplementedException();

        public void ShowShopScreen() => throw new NotImplementedException();

        public IDeckBuildingView GetDeckBuildingView() => throw new NotImplementedException();

        public void DestroyAllBuildings()
        {
        }

        public void ShowDryRunResults()
        {
            foreach (var (booster, allProb) in _allProbabilitiesBoosterViews)
            {
                if (!_lastFrameBoosterViews.TryGetValue(booster, out var lastFrame))
                    lastFrame = new DryRunBoosterView();

                if (!_noProbabilitiesBoosterViews.TryGetValue(booster, out var noProb))
                    noProb = new DryRunBoosterView();

                var boosterCard = _state.Boosters.Find(b => b.Id == booster);
                var realBoosterView = _realGameView.GetBooster(booster);
                BoosterComparer.Compare(boosterCard, realBoosterView, allProb, noProb, lastFrame);

                _lastFrameBoosterViews.Remove(booster);
            }

            foreach (var (building, view) in _allProbabilitiesBuildingViews)
            {
                if (!_lastFrameBuildingViews.TryGetValue(building, out var lastFrame))
                    lastFrame = new DryRunBuildingView(null);

                view.ApplyChanges(lastFrame);

                _lastFrameBuildingViews.Remove(building);
            }

            foreach (var (id, _) in _lastFrameBoosterViews)
            {
                HideAll(id);
            }

            foreach (var (_, view) in _lastFrameBuildingViews)
            {
                view.HideAll();
            }

            _lastFrameBoosterViews = _allProbabilitiesBoosterViews;
            _lastFrameBuildingViews = _allProbabilitiesBuildingViews;
            _allProbabilitiesBoosterViews = new Dictionary<BoosterInstanceId, DryRunBoosterView>();
            _allProbabilitiesBuildingViews = new Dictionary<BuildingId, DryRunBuildingView>();
            _noProbabilitiesBoosterViews.Clear();
            _noProbabilitiesBuildingViews.Clear();
        }

        public void Clear()
        {
            foreach (var (boosterId, _) in _lastFrameBoosterViews)
            {
                HideAll(boosterId);
            }

            foreach (var (_, view) in _lastFrameBuildingViews)
            {
                view.HideAll();
            }

            _lastFrameBoosterViews.Clear();
            _lastFrameBuildingViews.Clear();
            _allProbabilitiesBoosterViews.Clear();
            _allProbabilitiesBuildingViews.Clear();
            _noProbabilitiesBoosterViews.Clear();
            _noProbabilitiesBuildingViews.Clear();
        }

        private void HideAll(BoosterInstanceId boosterId)
        {
            var boosterView = _realGameView.GetBooster(boosterId);
            boosterView.GetMoneyVisualizer().HideDryRun();
            boosterView.GetScoringVisualizer().HideDryRun();
            boosterView.GetRetriggerVisualizer().HideDryRun();
            boosterView.GetResetVisualizer().HideDryRun();
            boosterView.GetScalingVisualizer().HideDryRun();
        }
    }
}