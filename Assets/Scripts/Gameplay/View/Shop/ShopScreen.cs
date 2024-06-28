﻿using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.DeckBuilding;
using RogueIslands.Gameplay.View.Boosters;
using RogueIslands.Gameplay.View.DeckBuilding;
using RogueIslands.UISystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RogueIslands.Gameplay.View.Shop
{
    public class ShopScreen : MonoBehaviour, IWindow
    {
        [SerializeField] private ShopItem _itemPrefab;
        [SerializeField] private BoosterCardView _boosterPrefab;
        [SerializeField] private ConsumableCardView _consumableCardView;
        [SerializeField] private Transform _cardParent;
        [SerializeField] private Button _continue, _reroll;
        [SerializeField] private TextMeshProUGUI _rerollText;

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
            
            UpdateRerollCost();
            
            for (var i = 0; i < Shop.CardCount; i++)
            {
                var shopIndex = i;
                
                var item = Instantiate(_itemPrefab, _cardParent);

                switch (Shop.ItemsForSale[i])
                {
                    case BoosterCard booster:
                        InstantiateBoosterCard(item, booster);
                        break;
                    case Consumable consumable:
                        InstantiateConsumableCard(item, consumable);
                        break;
                }
                

                item.SetPrice($"${Shop.ItemsForSale[i].BuyPrice}");
                item.BuyClicked += () =>
                {
                    if (GameManager.Instance.State.Money < Shop.ItemsForSale[shopIndex].BuyPrice)
                        return;

                    GameManager.Instance.State.PurchaseItemAtShop(GameManager.Instance, shopIndex);
                    
                    GameUI.Instance.RefreshMoney();
                    
                    Destroy(item.gameObject);
                };
            }
        }

        private void InstantiateBoosterCard(ShopItem item, BoosterCard booster)
        {
            var card = Instantiate(_boosterPrefab, item.transform);
            card.Initialize(booster);
            Destroy(card.GetComponent<BoosterView>());
            Destroy(card.GetComponent<BoosterCardView>());
        }

        private void UpdateRerollCost()
        {
            _rerollText.text = $"Reroll (${Shop.CurrentRerollCost})";
        }

        private void OnRerollClicked()
        {
            if (GameManager.Instance.State.Money < Shop.CurrentRerollCost)
                return;
            
            GameManager.Instance.State.RerollShop(GameManager.Instance);
            
            PopulateShop();
        }

        private void OnContinueClicked()
        {
            GameManager.Instance.ShowRoundsSelectionScreen();
            Destroy(gameObject);
        }

        private void InstantiateConsumableCard(ShopItem item, Consumable consumable)
        {
            var card = Instantiate(_consumableCardView, item.transform);
            card.Initialize(consumable);
            Destroy(card.GetComponent<ConsumableCardView>());
        }
    }
}