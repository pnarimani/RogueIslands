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
        private Dictionary<BuildingId, DryRunBuildingView> _buildingViews = new();
        private Dictionary<BoosterInstanceId, DryRunBoosterView> _boosterViews = new();

        public DryRunGameView(IGameView realGameView) => _realGameView = realGameView;

        public IBuildingView GetBuilding(Building building)
        {
            if (!_buildingViews.TryGetValue(building.Id, out var view))
            {
                view = new DryRunBuildingView(_realGameView.GetBuilding(building));
                _buildingViews.Add(building.Id, view);
            }

            return view;
        }

        public IBoosterView GetBooster(IBooster booster)
        {
            if (!_boosterViews.TryGetValue(booster.Id, out var view))
            {
                var realBoosterView = _realGameView.GetBooster(booster);
                if (realBoosterView == null)
                    throw new Exception($"Failed to find booster view for booster {booster.Name}");

                view = new DryRunBoosterView(realBoosterView);
                _boosterViews.Add(booster.Id, view);
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
            foreach (var (booster, view) in _boosterViews)
            {
                if (!_lastFrameBoosterViews.TryGetValue(booster, out var lastFrame))
                    lastFrame = new DryRunBoosterView(null);

                view.ApplyChanges(lastFrame);

                _lastFrameBoosterViews.Remove(booster);
            }

            foreach (var (building, view) in _buildingViews)
            {
                if (!_lastFrameBuildingViews.TryGetValue(building, out var lastFrame))
                    lastFrame = new DryRunBuildingView(null);

                view.ApplyChanges(lastFrame);

                _lastFrameBuildingViews.Remove(building);
            }

            foreach (var (_, view) in _lastFrameBoosterViews)
            {
                view.HideAll();
            }
            
            foreach (var (_, view) in _lastFrameBuildingViews)
            {
                view.HideAll();
            }

            _lastFrameBoosterViews = _boosterViews;
            _lastFrameBuildingViews = _buildingViews;
            _boosterViews = new Dictionary<BoosterInstanceId, DryRunBoosterView>();
            _buildingViews = new Dictionary<BuildingId, DryRunBuildingView>();
        }

        public void Clear()
        {
            foreach (var (_, view) in _lastFrameBoosterViews)
            {
                view.HideAll();
            }
            
            foreach (var (_, view) in _lastFrameBuildingViews)
            {
                view.HideAll();
            }
            
            _lastFrameBoosterViews.Clear();
            _lastFrameBuildingViews.Clear();
            _boosterViews.Clear();
            _buildingViews.Clear();
        }
    }
}