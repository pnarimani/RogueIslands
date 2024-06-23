using Cysharp.Threading.Tasks;
using DG.Tweening;
using RogueIslands.Buildings;
using RogueIslands.View.Feedbacks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RogueIslands.View
{
    public class BuildingCardView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private RectTransform _animationParent;
        [SerializeField] private Image _colorBg, _colorGradient, _buildingIcon;
        [SerializeField] private LabelFeedback _moneyFeedback, _productFeedback, _multFeedback;

        private bool _isSelected;
        private BuildingView _buildingPreview;
        private Transform _originalParent;
        private CardListItem _cardListItem;

        private readonly BuildingViewFactory _buildingViewFactory = new();

        public Building Data { get; private set; }

        public void Initialize(Building data)
        {
            Data = data;

            _colorBg.color = _colorGradient.color = data.Color.Color;
            _buildingIcon.sprite = Resources.Load<Sprite>(data.IconAddress);

            GetComponent<DescriptionBoxSpawner>().Initialize(data);
        }

        private void Start()
        {
            _cardListItem = GetComponent<CardListItem>();
        }

        private void OnDestroy()
        {
            if (_buildingPreview != null)
                Destroy(_buildingPreview.gameObject);
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(1))
            {
                if (_isSelected)
                {
                    OnPointerClick(default);
                }
            }

            if (_isSelected && GameUI.Instance.IsInSpawnRegion(Input.mousePosition))
            {
                if (_buildingPreview == null)
                {
                    _buildingPreview = _buildingViewFactory.Create(Data);
                }

                _buildingPreview.transform.position =
                    BuildingViewPlacement.Instance.GetPosition(_buildingPreview.transform);

                var isValidPlacement = BuildingViewPlacement.Instance.IsValidPlacement(_buildingPreview.transform);
                foreach (var r in _buildingPreview.GetComponentsInChildren<Renderer>(true))
                    r.enabled = isValidPlacement;

                EffectRangeHighlighter.Highlight(_buildingPreview.transform.position, Data.Range,
                    _buildingPreview.gameObject);
                EffectRangeHighlighter.ShowRanges(true, _buildingPreview.gameObject);
                WorldBoosterBoundCheck.HighlightOverlappingWorldBoosters(_buildingPreview.transform);
            }
            else
            {
                if (_buildingPreview != null)
                {
                    Destroy(_buildingPreview.gameObject);
                    WorldBoosterBoundCheck.HideAllDeletionWarnings();
                    EffectRangeHighlighter.ShowRanges(false);
                    EffectRangeHighlighter.LowlightAll();
                }
            }
        }

        private void OnWorldClicked()
        {
            InputHandling.Instance.Click -= OnWorldClicked;

            if (GameUI.Instance.IsInSpawnRegion(Input.mousePosition) &&
                BuildingViewPlacement.Instance.IsValidPlacement(_buildingPreview.transform))
            {
                GameManager.Instance.State.PlaceBuilding(GameManager.Instance, Data,
                    _buildingPreview.transform.position,
                    Quaternion.identity);

                EffectRangeHighlighter.ShowRanges(false);
                EffectRangeHighlighter.LowlightAll();

                GameUI.Instance.RefreshMoney();

                Destroy(gameObject);
            }
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            if (GameManager.Instance.IsPlaying)
                return;

            _isSelected = !_isSelected;

            if (_isSelected)
            {
                _cardListItem.ShouldAnimateToTarget = false;

                transform.DOLocalMoveY(50, 0.2f)
                    .SetRelative(true)
                    .OnComplete(() => { InputHandling.Instance.Click += OnWorldClicked; });

                foreach (var c in FindObjectsByType<BuildingCardView>(FindObjectsInactive.Include,
                             FindObjectsSortMode.None))
                {
                    if (c == this) continue;

                    if (c._isSelected)
                        c.OnPointerClick(default);
                }
            }
            else
            {
                transform.DOLocalMoveY(-50, 0.2f)
                    .SetRelative(true)
                    .OnComplete(() => _cardListItem.ShouldAnimateToTarget = true);

                InputHandling.Instance.Click -= OnWorldClicked;
            }
        }

        public async UniTask BuildingMadeMoney(int money)
        {
            _moneyFeedback.SetText("$" + money);
            await _moneyFeedback.Play();
        }

        public async UniTask BuildingMadeProduct(double delta)
        {
            _productFeedback.SetText($"+{delta:F1}");
            await _productFeedback.Play();
        }

        public async UniTask BuildingMadeXMult(double delta)
        {
            _multFeedback.SetText($"x{delta:F1}");
            await _multFeedback.Play();
        }

        public async UniTask BuildingMadePlusMult(double delta)
        {
            _multFeedback.SetText($"+{delta:F1}");
            await _multFeedback.Play();
        }
    }
}