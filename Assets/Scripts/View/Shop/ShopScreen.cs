using System;
using UnityEngine;
using UnityEngine.UI;

namespace RogueIslands.View.Shop
{
    public class ShopScreen : MonoBehaviour
    {
        [SerializeField] private ShopItem _itemPrefab;
        [SerializeField] private BoosterView _boosterPrefab;
        [SerializeField] private Transform _cardParent;
        [SerializeField] private Button _continue, _reroll;

        private static ShopState Shop => GameManager.Instance.State.Shop;

        private void Awake()
        {
            _continue.onClick.AddListener(OnContinueClicked);
            _reroll.onClick.AddListener(OnRerollClicked);
            
            PopulateShop();
        }

        private void PopulateShop()
        {
            _cardParent.DestroyChildren();
            
            for (var i = 0; i < Shop.CardCount; i++)
            {
                var item = Instantiate(_itemPrefab, _cardParent);
                var booster = Shop.BoostersForSale[i];
                var boosterView = Instantiate(_boosterPrefab, item.transform);
                boosterView.Show(booster);

                var index = i;
                item.BuyClicked += () =>
                {
                    if (GameManager.Instance.State.Money < booster.BuyPrice)
                        return;

                    GameManager.Instance.State.PurchaseItemAtShop(GameManager.Instance, index);
                    
                    Destroy(item.gameObject);
                };
            }
        }

        private void OnRerollClicked()
        {
            if (GameManager.Instance.State.Money < Shop.CurrentRerollCost)
                return;
            
            GameManager.Instance.State.RerollShop();
        }

        private void OnContinueClicked()
        {
            GameManager.Instance.State.StartWeek(GameManager.Instance);
            
            Destroy(gameObject);
        }
    }
}