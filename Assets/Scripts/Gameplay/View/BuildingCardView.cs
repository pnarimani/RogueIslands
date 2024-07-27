using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Flexalon;
using RogueIslands.DependencyInjection;
using RogueIslands.Diagnostics;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.DryRun;
using RogueIslands.Gameplay.View.Descriptions;
using RogueIslands.Gameplay.View.Feedbacks;
using RogueIslands.Gameplay.View.Shop;
using RogueIslands.Gameplay.View.Win;
using RogueIslands.View.Audio;
using TMPro;
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
        [SerializeField] private TextMeshProUGUI _size, _category;

        private BuildingView _buildingPreview;
        private Transform _originalParent;
        private FlexalonObject _cardListItem;

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

            _size.text = data.Size switch
            {
                BuildingSize.Small => "S",
                BuildingSize.Medium => "M",
                BuildingSize.Large => "L",
                _ => throw new ArgumentOutOfRangeException()
            };

            _category.text = data.Category.Value;

            GetComponent<DescriptionBoxSpawner>().Initialize(data);
        }

        private void Start()
        {
            _audio = StaticResolver.Resolve<IBuildingCardAudio>();
            _cardListItem = GetComponent<FlexalonObject>();
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
                using (new ProfilerBlock("BuildingCardView.CardSelected"))
                {
                    if (_buildingPreview == null)
                    {
                        _buildingPreview = _buildingViewFactory.Create(Data);
                        _buildingPreview.IsPreview = true;
                        Destroy(_buildingPreview.GetComponent<DescriptionBoxSpawner>());
                    }

                    _buildingPreview.transform.position =
                        BuildingViewPlacement.Instance.GetPosition(_buildingPreview);
                    _buildingPreview.Data.Position = _buildingPreview.transform.position;

                    var isValidPlacement = BuildingViewPlacement.Instance.IsValidPlacement(_buildingPreview);
                    _buildingPreview.ShowValidPlacement(isValidPlacement);

                    EffectRangeHighlighter.HighlightBuilding(_buildingPreview);
                    WorldBoosterBoundCheck.HighlightOverlappingWorldBoosters(_buildingPreview.transform);

                    StaticResolver.Resolve<DryRunScoringController>().ExecuteDryRun(_buildingPreview.Data);
                }
            }
            else
            {
                if (_buildingPreview != null)
                {
                    Destroy(_buildingPreview.gameObject);
                    WorldBoosterBoundCheck.HideAllDeletionWarnings();
                    EffectRangeHighlighter.LowlightAll();
                    StaticResolver.Resolve<DryRunScoringController>().Clear();
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
                BuildingViewPlacement.Instance.IsValidPlacement(_buildingPreview))
            {
                GameUI.Instance.HideDeck();

                PlayButtonHandler.Instance.PlaceBuildingDown(Data, _buildingPreview.transform.position,
                    _buildingPreview.transform.rotation, CancellationToken.None).Forget();

                EffectRangeHighlighter.LowlightAll();
                StaticResolver.Resolve<DryRunScoringController>().Clear();

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

            if (FindObjectOfType<ShopScreen>() || FindObjectOfType<RoundWinScreen>())
                return;

            IsSelected = !IsSelected;

            if (IsSelected)
            {
                _audio.PlayCardSelected();
                GetComponent<DescriptionBoxSpawner>().SpawnManually();

                _cardListItem.Offset = new Vector3(0, 50);
                InputHandling.Instance.Scroll += OnInputRotateBuilding;
                InputHandling.Instance.Click += OnWorldClicked;

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
                GetComponent<DescriptionBoxSpawner>().HideManually();

                _cardListItem.Offset = new Vector3(0, 0);

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