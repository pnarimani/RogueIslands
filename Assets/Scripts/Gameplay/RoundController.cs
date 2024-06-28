using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.GameEvents;
using RogueIslands.Gameplay.Rollback;

namespace RogueIslands.Gameplay
{
    public class RoundController
    {
        private readonly GameState _state;
        private readonly IGameView _view;
        private readonly IEventController _eventController;
        private readonly WorldBoosterGeneration _worldBoosterGeneration;
        private readonly ResetController _resetController;

        public RoundController(GameState state, IGameView view, IEventController eventController,
            WorldBoosterGeneration worldBoosterGeneration, ResetController resetController)
        {
            _resetController = resetController;
            _worldBoosterGeneration = worldBoosterGeneration;
            _eventController = eventController;
            _view = view;
            _state = state;
        }

        public void TryEndingRound()
        {
            if (HasLost())
            {
                _state.Result = GameResult.Lose;
                _view.ShowLoseScreen();
                return;
            }

            if (IsRoundFinished())
            {
                _eventController.Execute(new RoundEnd());

                _state.PopulateShop();

                _state.Round++;
                if (_state.Round >= GameState.RoundsPerAct)
                {
                    _state.Round = 0;
                    _state.Act++;

                    _eventController.Execute(new ActEnd());
                }

                if (_state.Act >= GameState.TotalActs)
                {
                    _state.Result = GameResult.Win;
                    _view.ShowGameWinScreen();
                }
                else
                {
                    ShowRoundWinScreen();
                }
            }
            else
            {
                _state.Buildings.HandPointer += _state.HandSize;
                _view.ShowBuildingsInHand();

                _worldBoosterGeneration.GenerateWorldBoosters();
            }
        }

        private void ShowRoundWinScreen()
        {
            var winScreen = _view.ShowRoundWin();
            winScreen.AddMoneyChange(new MoneyChange
            {
                Change = _state.MoneyPayoutPerRound,
                Reason = "Round Completion Prize",
            });

            winScreen.AddMoneyChange(new MoneyChange
            {
                Change = _state.TotalDays - _state.Day,
                Reason = "Days Left (1$ per day)",
            });

            foreach (var change in _state.MoneyChanges)
            {
                winScreen.AddMoneyChange(change);
            }
        }

        public void ClaimRoundEndMoney()
        {
            _state.Money += _state.MoneyPayoutPerRound;
            _state.Money += _state.TotalDays - _state.Day;
            foreach (var change in _state.MoneyChanges)
                _state.Money += change.Change;
            _state.MoneyChanges.Clear();

            _view.GetUI().RefreshMoney();
            _view.ShowShopScreen();
        }

        public void StartRound()
        {
            _view.DestroyWorldBoosters();
            _state.WorldBoosters.SpawnedBoosters.Clear();

            _worldBoosterGeneration.GenerateWorldBoosters();

            _resetController.RestoreProperties();

            _state.ShuffleDeck();
            RemoveBuildingsClusterId();
            _state.CurrentScore = 0;
            _state.Day = 0;

            _eventController.Execute(new RoundStart());

            _view.DestroyBuildings();
            _state.Buildings.HandPointer = 0;
            _view.ShowBuildingsInHand();

            _view.GetUI().RefreshAll();
        }

        private void RemoveBuildingsClusterId()
        {
            foreach (var building in _state.Buildings.Deck)
                building.ClusterId = default;
        }

        private bool HasLost()
            => _state.Day >= _state.TotalDays && _state.CurrentScore < _state.GetCurrentRequiredScore();

        private bool IsRoundFinished()
        {
            return _state.CurrentScore >= _state.GetCurrentRequiredScore();
        }
    }
}