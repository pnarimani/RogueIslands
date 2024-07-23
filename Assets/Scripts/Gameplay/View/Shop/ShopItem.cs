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
        [SerializeField] private Transform _spawnParent;
        
        public GameObject InnerObject { get; set; }

        public Transform SpawnParent
        {
            get => _spawnParent;
        }

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