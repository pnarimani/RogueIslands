using System;
using System.Linq;
using RogueIslands.Buildings;
using UnityEngine;
using UnityEngine.UI;

namespace RogueIslands.View.DeckPreview
{
    public class DeckPreviewScreen : MonoBehaviour
    {
        [SerializeField] private BuildingCardView _buildingCard;
        [SerializeField] private CardListView _blue, _red, _green, _purple;
        [SerializeField] private Button _close;
        
        private void Start()
        {
            _close.onClick.AddListener(() => Destroy(gameObject));
            
            var deck = GameManager.Instance.State.BuildingDeck.Deck
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