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

        public Building Data { get; private set; }

        public void Show(Building data)
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
                    _instance = Instantiate(Resources.Load<BuildingView>(Data.PrefabAddress));
                    _instance.SetData(Data);
                    _instance.ShowSynergyRange(true);
                }

                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit, 100, LayerMask.GetMask("Ground")))
                {
                    _instance.transform.position = hit.point;

                    var buildings =
                        FindObjectsByType<BuildingView>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                    foreach (var b in buildings)
                        b.ShowSynergyRange(true);

                    if (_instanceNeighbours != null)
                    {
                        foreach (var neighbour in _instanceNeighbours)
                        {
                            if (neighbour != null)
                                neighbour.HighlightConnection(false);
                        }
                    }

                    _instanceNeighbours = GameManager.Instance.State.GetIslands(hit.point, Data.Range)
                        .SelectMany(x => x)
                        .Select(placedBuilding => buildings.First(b => b.Data == placedBuilding))
                        .ToList();

                    foreach (var n in _instanceNeighbours)
                        n.HighlightConnection(true);
                }
                else
                {
                    Destroy(_instance.gameObject);
                }
            }
            else
            {
                if (_instance != null)
                {
                    Destroy(_instance.gameObject);
                }
            }
        }

        private void InputHandlerOnClick()
        {
            InputHandling.Instance.Click -= InputHandlerOnClick;

            if (!_isSelected)
                throw new Exception("wtfff");

            if (_ui.IsInSpawnRegion(Input.mousePosition))
            {
                if (CanAffordTheCard())
                {
                    var ray = _camera.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out var hit, 100, LayerMask.GetMask("Ground")))
                    {
                        var buildingData = GameManager.Instance.State.PlaceBuilding(GameManager.Instance, Data);
                        
                        var building = Instantiate(Resources.Load<BuildingView>(Data.PrefabAddress), hit.point, Quaternion.identity);
                        building.transform.DOMoveY(1, 0.3f)
                            .From()
                            .SetRelative(true)
                            .SetEase(Ease.OutBounce);
                        building.SetData(buildingData);

                        GameUI.Instance.Refresh();

                        Destroy(gameObject);
                    }
                }
                else
                {
                    _ui.ShowNotEnoughEnergy();
                }
            }
        }


        private bool CanAffordTheCard()
            => GameManager.Instance.State.Energy >= Data.EnergyCost;

        public void OnPointerClick(PointerEventData eventData)
        {
            _isSelected = !_isSelected;

            if (_isSelected)
            {
                _cardListItem.ShouldAnimateToTarget = false;

                transform.DOLocalMoveY(50, 0.2f)
                    .SetRelative(true)
                    .OnComplete(() => { InputHandling.Instance.Click += InputHandlerOnClick; });

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

                InputHandling.Instance.Click -= InputHandlerOnClick;

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