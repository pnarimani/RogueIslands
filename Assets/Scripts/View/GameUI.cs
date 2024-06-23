using System;
using RogueIslands.Boosters;
using RogueIslands.Particles;
using RogueIslands.View.Boosters;
using UnityEngine;
using UnityEngine.UI;

namespace RogueIslands.View
{
    public class GameUI : SingletonMonoBehaviour<GameUI>, IGameUI
    {
        [SerializeField] private NumberText 
            _products,
            _multiplier,
            _requiredOutput,
            _currentAmount,
            _budget,
            _days,
            _week,
            _month,
            _boosterSlots;

        [SerializeField] private Button _playButton;
        [SerializeField] private BuildingCardView _buildingCardPrefab;
        [SerializeField] private BoosterView _boosterPrefab;
        [SerializeField] private CardListView _buildingCardList, _boosterList;
        [SerializeField] private GameObject _notEnoughEnergy;
        [SerializeField] private ParticleSystemTarget _productTarget;

        public event Action PlayClicked;

        public ParticleSystemTarget ProductTarget => _productTarget;

        private void Start()
        {
            _playButton.onClick.AddListener(() => PlayClicked?.Invoke());
        }

        public void ShowBuildingCard(Building building)
        {
            var card = Instantiate(_buildingCardPrefab, _buildingCardList.Content);
            card.Initialize(building);
            _buildingCardList.Add(card.GetComponent<CardListItem>());
        }

        public void ShowBoosterCard(BoosterCard booster)
        {
            var card = Instantiate(_boosterPrefab, _boosterList.Content);
            card.Initialize(booster);
            _boosterList.Add(card.GetComponent<CardListItem>());
        }

        public bool IsInSpawnRegion(Vector3 screenPosition)
        {
            var corners = new Vector3[4];
            ((RectTransform)_buildingCardList.transform).GetWorldCorners(corners);
            var bl = corners[0];
            var tr = corners[2];
            var viewRect = new Rect(bl, tr - bl);
            return !viewRect.Contains(screenPosition);
        }

        public void RefreshAll()
        {
            RefreshScores();
            RefreshMoneyAndEnergy();
            RefreshDate();
            RefreshBoosters();
        }

        private void RefreshBoosters()
        {
            _boosterSlots.UpdateNumber(GameManager.Instance.State.Boosters.Count);
            _boosterSlots.SetMax(GameManager.Instance.State.MaxBoosters);
        }

        public void RefreshMoneyAndEnergy()
        {
            var state = GameManager.Instance.State;
            _budget.UpdateNumber(state.Money);
        }

        public void RefreshScores()
        {
            var state = GameManager.Instance.State;
            
            if (state.ScoringState != null)
            {
                _products.UpdateNumber(state.ScoringState.Products);
                _multiplier.UpdateNumber(state.ScoringState.Multiplier);
            }
            else
            {
                _products.SetNumber(0);
                _multiplier.SetNumber(1);
            }

            _requiredOutput.UpdateNumber(state.GetCurrentRequiredScore());
            _currentAmount.UpdateNumber(state.CurrentScore);
        }

        public void RefreshDate()
        {
            var state = GameManager.Instance.State;
            _days.UpdateNumber(state.TotalDays - state.Day);
            _week.UpdateNumber(state.Round + 1);
            _month.UpdateNumber(state.Act + 1);
        }

        public void ProductBoosted(double count)
        {
            _products.UpdateNumber(_products.CurrentNumber + count);
        } 

        public void MultBoosted(double multBoost)
        {
            _multiplier.UpdateNumber(multBoost);
        }
    }
}