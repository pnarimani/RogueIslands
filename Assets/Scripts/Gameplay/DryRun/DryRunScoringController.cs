using System;
using Autofac;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Serialization;
using UnityEngine.Profiling;

namespace RogueIslands.Gameplay.DryRun
{
    public class DryRunScoringController : IDisposable
    {
        private readonly ICloner _cloner;
        private readonly GameState _fakeGame;
        private readonly GameState _realGame;
        private readonly DryRunGameView _fakeView;
        private readonly ILifetimeScope _scope;
        private readonly ScoringController _scoringController;

        public DryRunScoringController(
            ILifetimeScope container,
            GameState realGame,
            ICloner cloner ,
            DryRunGameView fakeView)
        {
            _fakeView = fakeView;
            _realGame = realGame;
            _fakeGame = cloner.Clone(realGame);
            _cloner = cloner;
            _realGame = realGame;
            
            _scope = container.BeginLifetimeScope(builder =>
            {
                builder.RegisterInstance(_fakeGame);
                builder.RegisterInstance(_fakeView).As<IGameView>();
            });

            _scoringController = _scope.Resolve<ScoringController>();
        }

        public void ExecuteDryRun(Building building)
        {
            Profiler.BeginSample("DryRunScoringController.ExecuteDryRun");

            var clonedBuilding = _cloner.Clone(building);
            _cloner.CloneTo(_realGame.Boosters, _fakeGame.Boosters);
            _cloner.CloneTo(_realGame.Buildings.PlacedDownBuildings, _fakeGame.PlacedDownBuildings);
            _fakeGame.Money = _realGame.Money;
            _fakeGame.CurrentScore = _realGame.CurrentScore;

            _fakeGame.Buildings.PlacedDownBuildings.Add(clonedBuilding);
            _scoringController.ScoreBuilding(clonedBuilding);

            _fakeView.ShowDryRunResults();
            
            Profiler.EndSample();
        }

        public void Clear()
        {
            _fakeView.Clear();
        }

        public void Dispose()
        {
            _scope?.Dispose();
        }
    }
}