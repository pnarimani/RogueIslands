using System;
using UnityEngine;
using UnityEngine.UI;

namespace RogueIslands.View.Shop
{
    public class ShopItem : MonoBehaviour
    {
        [SerializeField] private Button _buy;

        public event Action BuyClicked;

        private void Awake()
        {
            _buy.onClick.AddListener(() => BuyClicked?.Invoke());
        }
    }
}