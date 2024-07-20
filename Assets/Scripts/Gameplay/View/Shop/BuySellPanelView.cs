using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RogueIslands.Gameplay.View.Shop
{
    public class BuySellPanelView : MonoBehaviour
    {
        [SerializeField] private Button _sell, _buy;
        [SerializeField] private TextMeshProUGUI _buyText, _sellText;

        public void ShowSellButton(string text, Action clicked)
        {
            _sellText.text = text;
            _sell.onClick.AddListener(() => clicked());
            _sell.gameObject.SetActive(true);
        }

        public void HideBuyButton()
        {
            _buy.gameObject.SetActive(false);
        }

        public void HideSellButton()
        {
            _sell.gameObject.SetActive(false);
        }
        
        public void ShowBuyButton(string text, Action clicked)
        {
            _buyText.text = text;
            _buy.onClick.AddListener(() => clicked());
            _buy.gameObject.SetActive(true);
        }
    }
}