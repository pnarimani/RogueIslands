using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RogueIslands.View
{
    public class BuildingCardView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI _name, _description;
        [SerializeField] private RectTransform _animationParent;

        private GameUI _ui;
        private bool _isSelected;
        private BuildingView _instance;
        private Transform _originalParent;
        private CardListItem _cardListItem;
        private List<BuildingView> _instanceNeighbours;
        private Camera _camera;

        private BuildingViewFactory _buildingViewFactory = new();

        public Building Data { get; private set; }

        public void Initialize(Building data)
        {
            Data = data;

            _name.text = data.Name;
            _description.text = data.Description;
        }

        private void Start()
        {
            _camera = Camera.main;
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
                    _instance.ShowSynergyRange(true);
                }

                _instance.transform.position = BuildingViewPlacement.Instance.GetPosition(_instance.transform);

                var buildingViews = FindObjectsByType<BuildingView>(FindObjectsSortMode.None);
                foreach (var b in buildingViews)
                    b.ShowSynergyRange(true);

                if (_instanceNeighbours != null)
                {
                    foreach (var neighbour in _instanceNeighbours)
                    {
                        if (neighbour != null)
                            neighbour.HighlightConnection(false);
                    }
                }

                _instanceNeighbours = GameManager.Instance.State
                    .GetIslands(_instance.transform.position, Data.Range)
                    .SelectMany(x => x)
                    .Select(buildingData => buildingViews.First(view => view.Data == buildingData))
                    .ToList();

                foreach (var n in _instanceNeighbours)
                    n.HighlightConnection(true);
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

            if (_ui.IsInSpawnRegion(Input.mousePosition))
            {
                GameManager.Instance.State.PlaceBuilding(GameManager.Instance, Data, _instance.transform.position,
                        Quaternion.identity);

                foreach (var b in FindObjectsByType<BuildingView>(FindObjectsInactive.Include,
                             FindObjectsSortMode.None))
                {
                    b.ShowSynergyRange(false);
                    b.HighlightConnection(false);
                }

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

                foreach (var b in FindObjectsByType<BuildingView>(FindObjectsInactive.Include,
                             FindObjectsSortMode.None))
                {
                    b.ShowSynergyRange(false);
                    b.HighlightConnection(false);
                }
            }
        }
    }
}