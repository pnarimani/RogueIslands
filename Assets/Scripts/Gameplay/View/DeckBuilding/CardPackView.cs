using System;
using RogueIslands.Gameplay.Buildings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RogueIslands.Gameplay.View.DeckBuilding
{
    public class CardPackView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _categoryText;
        [SerializeField] private Button _button;
        
        public event Action Clicked;

        private void Awake()
        {
            _button.onClick.AddListener(() => Clicked?.Invoke());
        }

        public void Initialize(Category category)
        {
            _categoryText.text = category.ToString();
        }

    }
}