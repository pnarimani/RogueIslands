using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RogueIslands.Gameplay.View.Shop
{
    public class ShopItem : MonoBehaviour
    {
        [SerializeField] private Button _buy;
        [SerializeField] private TextMeshProUGUI _price;
        
        public GameObject InnerObject { get; set; }
        
        public event Action BuyClicked;

        private void Awake()
        {
            _buy.onClick.AddListener(() => BuyClicked?.Invoke());
        }
        
        public void SetPrice(string price)
        {
            _price.text = price;
        }

        public void ShowBuyButton(bool active)
        {
            _buy.gameObject.SetActive(active);
        }
    }
}