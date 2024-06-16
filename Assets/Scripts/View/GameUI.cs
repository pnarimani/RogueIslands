using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using RogueIslands.Boosters;
using RogueIslands.Particles;
using UnityEngine;
using UnityEngine.UI;

namespace RogueIslands.View
{
    public class GameUI : SingletonMonoBehaviour<GameUI>, IGameUI
    {
        [SerializeField] private NumberText _products,
            _multiplier,
            _requiredOutput,
            _currentAmount,
            _budget,
            _energy,
            _days,
            _week,
            _month;

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
            var card = Instantiate(_buildingCardPrefab, _buildingCardList.transform);
            card.Initialize(building);
            _buildingCardList.Add(card.GetComponent<CardListItem>());
        }

        public void ShowBoosterCard(Booster booster)
        {
            var card = Instantiate(_boosterPrefab, _boosterList.transform);
            card.Show(booster);
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
        }

        public void RefreshMoneyAndEnergy()
        {
            var state = GameManager.Instance.State;
            _budget.UpdateNumber(state.Money);
            _energy.UpdateNumber(state.Energy);
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

            _requiredOutput.UpdateNumber(state.RequiredScore);
            _currentAmount.UpdateNumber(state.CurrentScore);
        }

        public void RefreshDate()
        {
            var state = GameManager.Instance.State;
            _days.UpdateNumber(state.TotalDays - state.Day);
            _week.UpdateNumber(state.Week + 1);
            _month.UpdateNumber(state.Month + 1);
        }

        public void ShowNotEnoughEnergy()
        {
            _energy.transform.DOShakePosition(0.2f, 10, 20);

            _notEnoughEnergy.transform.DOComplete();
            _notEnoughEnergy.SetActive(true);
            _notEnoughEnergy.transform.localScale = Vector3.zero;
            _notEnoughEnergy.transform.DOScale(Vector3.one, 0.2f)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    _notEnoughEnergy.transform.DOScale(0, .2f)
                        .SetDelay(1)
                        .SetEase(Ease.InBack)
                        .OnComplete(() => _notEnoughEnergy.SetActive(false));
                });
        }

        public void ProductBoosted(double count)
        {
            _products.UpdateNumber(_products.CurrentNumber + count);
        }

        public void RemoveBoosterCard(Booster booster)
        {
            Destroy(FindObjectsByType<BoosterView>(FindObjectsSortMode.None)
                .First(b => b.Data == booster)
                .gameObject);
        }

        public IBoosterView GetBoosterCard(Booster booster) 
            => FindObjectsByType<BoosterView>(FindObjectsSortMode.None).FirstOrDefault(b => b.Data == booster);

        public void MultBoosted(double multBoost)
        {
            _multiplier.UpdateNumber(multBoost);
        }
    }
}