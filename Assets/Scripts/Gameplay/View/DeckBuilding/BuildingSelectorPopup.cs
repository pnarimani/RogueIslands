using System;
using System.Collections.Generic;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.DeckBuilding;
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
        [SerializeField] private Button _submit;
        [SerializeField] private RectTransform[] _slots;
        [SerializeField] private CardListView _buildingCardList;

        private readonly List<Building> _selectedBuildings = new();
        private Consumable _consumable;

        public event Action<IReadOnlyList<Building>> BuildingsSelected;

        private void Awake()
        {
            _submit.onClick.AddListener(() =>
            {
                BuildingsSelected?.Invoke(_selectedBuildings);
                GameUI.Instance.RefreshDeckText();
                Destroy(gameObject);
            });

            var buildings = new List<Building>();
            var state = GameManager.Instance.State;
            ref var rand = ref state.DeckBuilding.BuildingSelectionRandom;
            var desiredCount = Mathf.Min(state.HandSize, state.Buildings.Deck.Count);
            while (buildings.Count < desiredCount)
            {
                var b = state.Buildings.Deck.SelectRandom(ref rand);
                if (!buildings.Contains(b))
                    buildings.Add(b);
            }

            foreach (var b in buildings)
            {
                var card = Instantiate(_buildingCardPrefab, _buildingCardList.Content);
                card.Initialize(b);
                var cardListItem = card.GetComponent<CardListItem>();
                cardListItem.AllowReorder = true;
                cardListItem.DragEnded += () =>
                {
                    foreach (var slot in _slots)
                    {
                        if (slot.childCount > 0)
                        {
                            continue;
                        }
                        
                        if (slot.GetWorldRect().Overlaps(cardListItem.transform.GetWorldRect()))
                        {
                            _buildingCardList.Remove(cardListItem);
                            cardListItem.transform.SetParent(slot, false);
                            cardListItem.transform.localPosition = Vector3.zero;
                            cardListItem.ShouldAnimateToTarget = false;
                            _selectedBuildings.Add(b);
                        }
                    }
                };
                Destroy(card);
                _buildingCardList.Add(cardListItem);
            }
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
            
            for (var i = 0; i < _slots.Length; i++)
            {
                _slots[i].gameObject.SetActive(i < consumable.Action.MaxCardsRequired);
            }
        }
    }
}