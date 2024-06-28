﻿using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RogueIslands.Gameplay.View.Shop
{
    public class ShopItem : MonoBehaviour
    {
        [SerializeField] private Button _buy;
        [SerializeField] private TextMeshProUGUI _price;

        public event Action BuyClicked;

        private void Awake()
        {
            _buy.onClick.AddListener(() => BuyClicked?.Invoke());
        }
        
        public void SetPrice(string price)
        {
            _price.text = price;
        }
    }
}