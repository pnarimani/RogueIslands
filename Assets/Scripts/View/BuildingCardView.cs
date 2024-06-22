using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RogueIslands.View
{
    public class BuildingCardView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI _name, _description;
        [SerializeField] private RectTransform _animationParent;
        [SerializeField] private Image _colorBg, _colorGradient, _buildingIcon;
        
        private GameUI _ui;
        private bool _isSelected;
        private BuildingView _instance;
        private Transform _originalParent;
        private CardListItem _cardListItem;

        private BuildingViewFactory _buildingViewFactory = new();

        public Building Data { get; private set; }

        public void Initialize(Building data)
        {
            Data = data;

            _colorBg.color = _colorGradient.color = data.Color.Color;
            _buildingIcon.sprite = Resources.Load<Sprite>(data.IconAddress);
            // _name.text = data.Name;
            // _description.text = data.Description;
        }

        private void Start()
        {
            _ui = FindFirstObjectByType<GameUI>();
            _cardListItem = GetComponent<CardListItem>();
        }

        private void OnDestroy()
        {
            if (_instance != null)
                Destroy(_instance.gameObject);
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

            if (_isSelected && _ui.IsInSpawnRegion(Input.mousePosition))
            {
                if (_instance == null)
                {
                    _instance = _buildingViewFactory.Create(Data);
                }

                _instance.transform.position = BuildingViewPlacement.Instance.GetPosition(_instance.transform);

                var isValidPlacement = BuildingViewPlacement.Instance.IsValidPlacement(_instance.transform);
                foreach (var r in _instance.GetComponentsInChildren<Renderer>(true))
                    r.enabled = isValidPlacement;

                EffectRangeHighlighter.Highlight(_instance.transform.position, Data.Range, _instance.gameObject);
                EffectRangeHighlighter.ShowRanges(true, _instance.gameObject);
            }
            else
            {
                if (_instance != null)
                {
                    Destroy(_instance.gameObject);
                }
            }
        }

        private void OnWorldClicked()
        {
            InputHandling.Instance.Click -= OnWorldClicked;

            if (_ui.IsInSpawnRegion(Input.mousePosition) && BuildingViewPlacement.Instance.IsValidPlacement(_instance.transform))
            {
                GameManager.Instance.State.PlaceBuilding(GameManager.Instance, Data, _instance.transform.position,
                    Quaternion.identity);

                EffectRangeHighlighter.ShowRanges(false);
                EffectRangeHighlighter.LowlightAll();

                GameUI.Instance.RefreshMoneyAndEnergy();

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

                EffectRangeHighlighter.ShowRanges(false);
                EffectRangeHighlighter.LowlightAll();
            }
        }
    }
}