using System;
using System.Collections.Generic;
using System.Linq;
using Flexalon;
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
        [SerializeField] private Transform _blue, _red, _green, _purple;
        [SerializeField] private TextMeshProUGUI _blueCount, _redCount, _greenCount, _purpleCount, _deckStatPrefab;
        [SerializeField] private Transform _deckStatParent;
        [SerializeField] private Button _close;
        private static readonly Vector3 _cardScale = Vector3.one * 0.6f;

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

            _blueCount.text = _blue.childCount.ToString();
            _redCount.text = _red.childCount.ToString();
            _greenCount.text = _green.childCount.ToString();
            _purpleCount.text = _purple.childCount.ToString();

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

            ContainAllItems(_blue);
            ContainAllItems(_red);
            ContainAllItems(_green);
            ContainAllItems(_purple);
        }

        private void ContainAllItems(Transform content)
        {
            if (content.childCount == 0)
                return;
            
            var layout = content.GetComponent<FlexalonFlexibleLayout>();
            Canvas.ForceUpdateCanvases();
            layout.ForceUpdate();

            var width = ((RectTransform)layout.transform).sizeDelta.x - 50;
            var spaceAvailablePerItem = width / content.childCount;
            var child = (RectTransform)content.GetChild(0);
            var itemWidth = child.rect.width * _cardScale.x;

            layout.Gap = spaceAvailablePerItem - itemWidth;
        }

        private void ShowStat(string title, int count)
        {
            var stat = Instantiate(_deckStatPrefab, _deckStatParent);
            stat.text = $"{title}: {count}";
        }

        private void AddCard(Building building, Transform list)
        {
            var card = Instantiate(_buildingCard, list);
            card.Initialize(building);
            card.CanPlaceBuildings = false;
            card.GetComponent<FlexalonObject>().Scale = _cardScale;
        }
    }
}