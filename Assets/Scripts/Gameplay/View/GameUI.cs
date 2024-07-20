using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Flexalon;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.View.Boosters;
using RogueIslands.Gameplay.View.Feedbacks;
using RogueIslands.UISystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RogueIslands.Gameplay.View
{
    public class GameUI : SingletonMonoBehaviour<GameUI>, IGameUI, IWindow
    {
        [SerializeField] private NumberText
            _budget,
            _week,
            _month;

        [SerializeField] private Button _deckButton, _optionsButton;
        [SerializeField] private BuildingCardView _buildingCardPrefab;
        [SerializeField] private BoosterCardView _boosterPrefab;
        [SerializeField] private Transform _buildingCardList, _deckPeekList;
        [SerializeField] private Transform _boosterList;
        [SerializeField] private TextMeshProUGUI _deckCardCount, _scoreRequirements;
        [SerializeField] private Image _scoreFill;
        [SerializeField] private Transform _scoreParent;
        [SerializeField] private LabelFeedback _productFeedback;
        
        private double _currentScore;

        private void Start()
        {
            _deckButton.onClick.AddListener(() => GameManager.Instance.ShowDeckPreview());
            _optionsButton.onClick.AddListener(() => GameManager.Instance.ShowOptions());
        }

        public void ShowBuildingCard(Building building)
        {
            var card = Instantiate(_buildingCardPrefab, _buildingCardList);
            card.Initialize(building);
        }

        public void ShowBuildingCardPeek(Building building)
        {
            var card = Instantiate(_buildingCardPrefab, _deckPeekList);
            card.Initialize(building);
            card.CanPlaceBuildings = false;
            card.transform.SetAsFirstSibling();
        }

        public async void MoveCardToHand(Building building)
        {
            var card = ObjectRegistry.GetBuildingCards().First(b => b.Data == building);
            card.CanPlaceBuildings = true;
            card.transform.SetParent(_buildingCardList, true);

            var item = card.GetComponent<FlexalonLerpAnimator>();
            item.InterpolationSpeed *= 0.5f;

            await UniTask.WaitForSeconds(1f);

            item.InterpolationSpeed *= 2;
        }

        public void RemoveCard(Building building)
        {
            Destroy(ObjectRegistry.GetBuildingCards().First(b => b.Data == building).gameObject);
        }

        public void ShowBoosterCard(BoosterCard booster)
        {
            var card = Instantiate(_boosterPrefab, _boosterList);
            card.Initialize(booster);

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
        
        public void RefreshDeckText()
        {
            var state = GameManager.Instance.State;
            var total = state.Buildings.Deck.Count;
            var rem = total - state.Buildings.HandPointer;
            rem = Mathf.Max(rem, 0);
            _deckCardCount.text = $"{rem}/{total}";
        }

        public void RefreshMoney()
        {
            var state = GameManager.Instance.State;
            _budget.UpdateNumber(state.Money);
        }

        public void RefreshScores()
        {
            _currentScore = 0;

            _productFeedback.gameObject.SetActive(false);

            var total = GameManager.Instance.State.GetCurrentRequiredScore();
            var current = GameManager.Instance.State.CurrentScore;
            _scoreRequirements.text = $"{current:0.#}/{total}";
            _scoreFill.fillAmount = (float)(current / total);
            Bump(_scoreParent);
        }

        public void RefreshDate()
        {
            var state = GameManager.Instance.State;
            _week.UpdateNumber(state.Round + 1);
            _month.UpdateNumber(state.Act + 1);
        }

        public void ProductBoosted(double delta)
        {
            _currentScore += delta;
            
            _productFeedback.SetText($"+{_currentScore:0.#}");
            _productFeedback.gameObject.SetActive(true);
            _productFeedback.Show();

            Bump(_productFeedback.transform);
        }

        private void Bump(Transform target)
        {
            target.DOKill();
            target.DOPunchScale(Vector3.one * 0.2f, 0.1f)
                .OnComplete(() => target.localScale = Vector3.one);
            target.DOPunchRotation(new Vector3(0, 0, Random.Range(-5, 5)), 0.1f)
                .OnComplete(() => target.localEulerAngles = Vector3.zero);
        }

        public void MoneyBoosted(int delta)
        {
            _budget.UpdateNumber(_budget.CurrentNumber + delta);
        }

        public void ShowScoringPanel(bool show)
        {
        }
    }
}