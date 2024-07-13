using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using RogueIslands.DependencyInjection;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.View.Boosters;
using RogueIslands.UISystem;
using RogueIslands.View.Audio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RogueIslands.Gameplay.View
{
    public class GameUI : SingletonMonoBehaviour<GameUI>, IGameUI, IWindow
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

        [SerializeField] private Button _playButton, _deckButton, _optionsButton;
        [SerializeField] private BuildingCardView _buildingCardPrefab;
        [SerializeField] private BoosterCardView _boosterPrefab;
        [SerializeField] private CardListView _buildingCardList, _boosterList;
        [SerializeField] private TextMeshProUGUI _deckCardCount;
        [SerializeField] private RectTransform _scoringPanel;

        public Transform ProductTarget => _products.transform;

        private void Start()
        {
            _playButton.onClick.AddListener(() =>
                PlayButtonHandler.Instance.OnPlayClicked(destroyCancellationToken).Forget());
            _deckButton.onClick.AddListener(() => GameManager.Instance.ShowDeckPreview());
            _optionsButton.onClick.AddListener(() => GameManager.Instance.ShowOptions());
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
            
            RefreshBoosters();
            RefreshDate();
            RefreshMoney();
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
            RefreshMoney();
            RefreshDate();
            RefreshBoosters();
            RefreshDeckText();
        }

        public void RefreshDeckText()
        {
            var state = GameManager.Instance.State;
            var total = state.Buildings.Deck.Count;
            var rem = total - state.Buildings.HandPointer;
            rem = Mathf.Max(rem, 0);
            _deckCardCount.text = $"{rem}/{total}";
        }

        private void RefreshBoosters()
        {
            _boosterSlots.UpdateNumber(GameManager.Instance.State.Boosters.Count);
            _boosterSlots.SetMax(GameManager.Instance.State.MaxBoosters);
        }

        public void RefreshMoney()
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

        public void ProductBoosted(double delta)
        {
            _products.UpdateNumber(_products.CurrentNumber + delta);
            var t = (_products.CurrentNumber * _multiplier.CurrentNumber) /
                    GameManager.Instance.State.GetCurrentRequiredScore();
            var scoringAudio = StaticResolver.Resolve<IScoringAudio>();
            scoringAudio.PlayScoreSound((int)(t * scoringAudio.ClipCount));
        }

        public void MultBoosted(double mult)
        {
            _multiplier.UpdateNumber(mult);
        }

        public void MoneyBoosted(int delta)
        {
            _budget.UpdateNumber(_budget.CurrentNumber + delta);
        }

        public void ShowScoringPanel(bool show)
        {
            _scoringPanel.DOComplete();
            _scoringPanel.DOAnchorPosY(show ? 0 : _scoringPanel.rect.height, 0.5f)
                .SetEase(show ? Ease.OutBack : Ease.InBack);

            var btnTransform = (RectTransform)_playButton.transform;
            var value = btnTransform.anchoredPosition.x;
            value = Mathf.Abs(value);
            btnTransform.DOAnchorPosX(show ? -value : value, 0.5f)
                .SetEase(show ? Ease.OutBack : Ease.InBack);
        }
    }
}