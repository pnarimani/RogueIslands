﻿using System;
using System.Collections.Generic;
using System.Linq;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.View.DeckBuilding;
using RogueIslands.UISystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RogueIslands.Gameplay.View.DeckPreview
{
    public class DeckPreviewScreen : MonoBehaviour, IWindow
    {
        [SerializeField] private BuildingCardView _buildingCard;
        [SerializeField] private CardListView _blue, _red, _green, _purple;
        [SerializeField] private TextMeshProUGUI _blueCount, _redCount, _greenCount, _purpleCount, _deckStatPrefab;
        [SerializeField] private Transform _deckStatParent;
        [SerializeField] private Button _close;

        private void Start()
        {
            _close.onClick.AddListener(() => Destroy(gameObject));

            var deck = GameManager.Instance.State.Buildings.Deck
                .OrderBy(building => building, new BuildingComparer())
                .ToList();

            foreach (var building in deck)
            {
                if (building.Color == ColorTag.Blue)
                    AddCard(building, _blue);
                else if (building.Color == ColorTag.Red)
                    AddCard(building, _red);
                else if (building.Color == ColorTag.Green)
                    AddCard(building, _green);
                else if (building.Color == ColorTag.Purple)
                    AddCard(building, _purple);
                else
                    throw new ArgumentOutOfRangeException();
            }

            _blueCount.text = _blue.Content.childCount.ToString();
            _redCount.text = _red.Content.childCount.ToString();
            _greenCount.text = _green.Content.childCount.ToString();
            _purpleCount.text = _purple.Content.childCount.ToString();

            foreach (var category in Category.All)
            {
                ShowStat(category.Value, deck.Count(building => building.Category == category));
            }

            Instantiate(_deckStatPrefab, _deckStatParent).text = "---";
            
            ShowStat("Small", deck.Count(building => building.Size == BuildingSize.Small));
            ShowStat("Medium", deck.Count(building => building.Size == BuildingSize.Medium));
            ShowStat("Big", deck.Count(building => building.Size == BuildingSize.Large));

            Instantiate(_deckStatPrefab, _deckStatParent).text = "---";
            
            ShowStat("Total", deck.Count);
        }

        private void ShowCategoryStat(List<Building> deck, Category category)
        {
        }

        private void ShowStat(string title, int count)
        {
            var stat = Instantiate(_deckStatPrefab, _deckStatParent);
            stat.text = $"{title}: {count}";
        }

        private void AddCard(Building building, CardListView list)
        {
            var card = Instantiate(_buildingCard, list.Content);
            card.Initialize(building);
            card.transform.localScale = Vector3.one * 0.6f;
            list.Add(card.GetComponent<CardListItem>());
        }
    }
}