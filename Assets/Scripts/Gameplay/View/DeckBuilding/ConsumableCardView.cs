using RogueIslands.Gameplay.DeckBuilding;
using TMPro;
using UnityEngine;

namespace RogueIslands.Gameplay.View.DeckBuilding
{
    public class ConsumableCardView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name;

        public Consumable Card { get; private set; }

        public void Initialize(Consumable card)
        {
            Card = card;

            _name.text = card.Name;
        }
    }
}