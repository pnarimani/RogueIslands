using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.DeckBuilding;
using RogueIslands.Gameplay.View.Feedbacks;
using RogueIslands.UISystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RogueIslands.Gameplay.View.DeckBuilding
{
    public class BuildingSelectorPopup : MonoBehaviour, IWindow
    {
        [SerializeField] private BuildingCardView _buildingCardPrefab;
        [SerializeField] private TextMeshProUGUI _title, _description;
        [SerializeField] private Transform _selectorParent;
        [SerializeField] private List<ConsumableTargetSelection> _selectors;
        [SerializeField] private Button _submit;
        [SerializeField] private CardListView _buildingCardList;
        [SerializeField] private PopupOpeningFeedback _openingFeedback;

        private readonly List<Building> _selectedBuildings = new();
        private Consumable _consumable;
        private ConsumableTargetSelection _currentSelector;

        private void Awake()
        {
            _submit.onClick.AddListener(() => { OnSubmitClicked().Forget(); });
            _openingFeedback.PlayOpening();
        }

        private async UniTask OnSubmitClicked()
        {
            _submit.interactable = false;
            await _currentSelector.PlaySubmitAnimation(_consumable, _selectedBuildings);
            GameUI.Instance.RefreshDeckText();
            await _openingFeedback.PlayClosing();
            Destroy(gameObject);
        }

        private void Update()
        {
            _submit.interactable = _selectedBuildings.Count >= _consumable.Action.MinCardsRequired;
        }

        public void Show(Consumable consumable)
        {
            _consumable = consumable;
            _title.text = consumable.Name;
            _description.text = consumable.Description.Get(consumable);

            InitializeSelector(consumable);

            using var buildings = CreateBuildingList();

            foreach (var building in buildings)
            {
                var cardComponent = Instantiate(_buildingCardPrefab, _buildingCardList.Content);
                cardComponent.Initialize(building);
                cardComponent.CanPlaceBuildings = false;
                var cardListItem = cardComponent.GetComponent<CardListItem>();
                cardListItem.AllowReorder = true;
                cardListItem.DragEnded += () => OnCardDragEnded(cardListItem, building);
                _buildingCardList.Add(cardListItem);
            }
        }

        private void InitializeSelector(Consumable consumable)
        {
            var selectorPrefab = _selectors.Find(x => x.CanHandle(consumable));
            if (selectorPrefab == null)
            {
                Debug.LogError($"No handler found for consumable {consumable.Name}");
                return;
            }

            _currentSelector = Instantiate(selectorPrefab, _selectorParent, false);
        }

        private void OnCardDragEnded(CardListItem cardListItem, Building building)
        {
            foreach (var slot in _currentSelector.GetSlots())
            {
                if (slot.Transform.childCount > 0)
                {
                    continue;
                }

                if (slot.Transform.GetWorldRect().Overlaps(cardListItem.transform.GetWorldRect()))
                {
                    _buildingCardList.Remove(cardListItem);
                    cardListItem.transform.SetParent(slot.Transform, false);
                    cardListItem.transform.localPosition = Vector3.zero;
                    cardListItem.ShouldAnimateToTarget = false;
                    _selectedBuildings.Add(building);
                    return;
                }
            }

            if (cardListItem.Owner == null)
                _buildingCardList.Add(cardListItem);
        }

        private static PooledList<Building> CreateBuildingList()
        {
            var state = GameManager.Instance.State;
            ref var rand = ref state.DeckBuilding.BuildingSelectionRandom;
            var desiredCount = Mathf.Min(state.HandSize, state.Buildings.Deck.Count);

            var buildings = PooledList<Building>.CreatePooled();
            while (buildings.Count < desiredCount)
            {
                var b = state.Buildings.Deck.SelectRandom(ref rand);
                if (!buildings.Contains(b))
                    buildings.Add(b);
            }

            return buildings;
        }
    }
}