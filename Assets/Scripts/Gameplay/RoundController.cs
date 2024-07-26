﻿using System.Linq;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.GameEvents;
using RogueIslands.Gameplay.Shop;
using UnityEngine;

namespace RogueIslands.Gameplay
{
    public class RoundController
    {
        private readonly GameState _state;
        private readonly IGameView _view;
        private readonly IEventController _eventController;
        private readonly ShopItemSpawner _shopItemSpawner;

        public RoundController(
            GameState state,
            IGameView view,
            IEventController eventController,
            ShopItemSpawner shopItemSpawner)
        {
            _shopItemSpawner = shopItemSpawner;
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
                
                _state.CurrentScore = 0;
                
                _shopItemSpawner.PopulateShop();
                _shopItemSpawner.PopulateBuildings();

                _state.Round++;
                if (_state.Round >= GameState.RoundsPerAct)
                {
                    _state.Round = 0;
                    _state.Act++;
                    _eventController.Execute(new ActEnd());

                    _view.DestroyAllBuildings();
                    _state.Buildings.PlacedDownBuildings.Clear();
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
        }

        private void ShowRoundWinScreen()
        {
            var winScreen = _view.ShowRoundWin();
            winScreen.AddMoneyChange(new MoneyChange
            {
                Change = _state.MoneyPayoutPerRound,
                Reason = "Round Completion Prize",
            });

            winScreen.AddMoneyChange(new MoneyChange()
            {
                Change = GetInterestMoney(),
                Reason = "Interest (1$ interest per 5$ in the bank)",
            });

            foreach (var change in _state.MoneyChanges)
            {
                winScreen.AddMoneyChange(change);
            }
        }

        private int GetInterestMoney()
            => Mathf.Clamp(_state.Money, 0, 25) / 5;

        public void ClaimRoundEndMoney()
        {
            _state.Money += _state.MoneyPayoutPerRound;
            _state.Money += GetInterestMoney();
            foreach (var change in _state.MoneyChanges)
                _state.Money += change.Change;
            _state.MoneyChanges.Clear();

            _view.GetUI().RefreshMoney();
        }

        public void StartRound()
        {
            _eventController.Execute(new RoundStart());

            if (_state.Act == 0 && _state.Round == 0)
            {
                foreach (var hand in _state.BuildingsInHand)
                {
                    _view.GetUI().ShowBuildingCard(hand);
                }

                foreach (var peek in _state.DeckPeek)
                {
                    _view.GetUI().ShowBuildingCardPeek(peek);
                }
            }

            _view.GetUI().RefreshDate();
            _view.GetUI().RefreshMoney();
            _view.GetUI().RefreshScores();
            _view.GetUI().ShowStageInformation();
        }

        private bool HasLost()
            => !_state.BuildingsInHand.Any() && _state.CurrentScore < _state.GetCurrentRequiredScore();

        private bool IsRoundFinished()
        {
            return _state.CurrentScore >= _state.GetCurrentRequiredScore();
        }
    }
}