﻿using Cysharp.Threading.Tasks;
using DG.Tweening;
using RogueIslands.DependencyInjection;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.View.DeckBuilding;
using RogueIslands.Gameplay.View.Feedbacks;
using RogueIslands.View.Audio;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RogueIslands.Gameplay.View
{
    public class BuildingCardView : MonoBehaviour, IPointerClickHandler
    {
        private const float BuildingRotationStep = 45 * 0.5f;
        
        [SerializeField] private RectTransform _animationParent;
        [SerializeField] private Image _colorBg, _colorGradient, _buildingIcon;
        [SerializeField] private LabelFeedback _moneyFeedback, _productFeedback, _multFeedback;

        private BuildingView _buildingPreview;
        private Transform _originalParent;
        private CardListItem _cardListItem;

        private readonly BuildingViewFactory _buildingViewFactory = new();
        private IBuildingCardAudio _audio;

        public bool CanPlaceBuildings { get; set; } = true;

        public Building Data { get; private set; }
        public bool IsSelected { get; private set; }

        public void Initialize(Building data)
        {
            Data = data;

            _colorBg.color = _colorGradient.color = data.Color.Color;
            _buildingIcon.sprite = Resources.Load<Sprite>(data.IconAddress);

            GetComponent<DescriptionBoxSpawner>().Initialize(data);
        }

        private void Start()
        {
            _audio = StaticResolver.Resolve<IBuildingCardAudio>();
            _cardListItem = GetComponent<CardListItem>();
        }

        private void OnDestroy()
        {
            if (_buildingPreview != null)
                Destroy(_buildingPreview.gameObject);
            
            InputHandling.Instance.Click -= OnWorldClicked;
            InputHandling.Instance.Scroll -= OnInputRotateBuilding;
        }

        private void Update()
        {
            if (!CanPlaceBuildings)
                return;
            
            if (Input.GetMouseButtonUp(1))
            {
                if (IsSelected)
                {
                    OnPointerClick(default);
                }
            }

            if (IsSelected && GameUI.Instance.IsInSpawnRegion(Input.mousePosition))
            {
                if (_buildingPreview == null)
                {
                    _buildingPreview = _buildingViewFactory.Create(Data);
                }

                _buildingPreview.transform.position =
                    BuildingViewPlacement.Instance.GetPosition(_buildingPreview.transform);

                var isValidPlacement = BuildingViewPlacement.Instance.IsValidPlacement(_buildingPreview.transform);
                _buildingPreview.ShowValidPlacement(isValidPlacement);

                EffectRangeHighlighter.HighlightBuilding(_buildingPreview);
                WorldBoosterBoundCheck.HighlightOverlappingWorldBoosters(_buildingPreview.transform);
            }
            else
            {
                if (_buildingPreview != null)
                {
                    Destroy(_buildingPreview.gameObject);
                    WorldBoosterBoundCheck.HideAllDeletionWarnings();
                    EffectRangeHighlighter.LowlightAll();
                }
            }
        }

        private void OnWorldClicked()
        {
            if (!CanPlaceBuildings)
                return;
            
            if (_buildingPreview == null)
                return;

            if (GameUI.Instance.IsInSpawnRegion(Input.mousePosition) &&
                BuildingViewPlacement.Instance.IsValidPlacement(_buildingPreview.transform))
            {
                StaticResolver.Resolve<BuildingPlacement>().PlaceBuilding(
                    Data,
                    _buildingPreview.transform.position,
                    _buildingPreview.transform.rotation
                );

                EffectRangeHighlighter.LowlightAll();

                GameUI.Instance.RefreshMoney();

                Destroy(gameObject);
            }
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            if (!CanPlaceBuildings)
                return;
            
            if (PlayButtonHandler.Instance.IsPlaying)
                return;

            IsSelected = !IsSelected;

            if (IsSelected)
            {
                _audio.PlayCardSelected();

                _cardListItem.ShouldAnimateToTarget = false;

                transform.DOLocalMoveY(50, 0.2f)
                    .SetRelative(true)
                    .OnComplete(() =>
                    {
                        InputHandling.Instance.Scroll += OnInputRotateBuilding;
                        InputHandling.Instance.Click += OnWorldClicked;
                    });

                foreach (var c in ObjectRegistry.GetBuildingCards())
                {
                    if (c == this) continue;

                    if (c.IsSelected)
                        c.OnPointerClick(default);
                }
            }
            else
            {
                _audio.PlayCardDeselected();

                transform.DOLocalMoveY(-50, 0.2f)
                    .SetRelative(true)
                    .OnComplete(() => _cardListItem.ShouldAnimateToTarget = true);

                InputHandling.Instance.Click -= OnWorldClicked;
                InputHandling.Instance.Scroll -= OnInputRotateBuilding;
            }
        }

        private void OnInputRotateBuilding(float obj)
        {
            if (_buildingPreview == null)
                return;
            
            _buildingPreview.transform.Rotate(new Vector3(0, obj * BuildingRotationStep, 0));
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