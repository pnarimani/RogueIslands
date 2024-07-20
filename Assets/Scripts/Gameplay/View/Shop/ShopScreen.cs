using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using RogueIslands.DependencyInjection;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.DeckBuilding;
using RogueIslands.Gameplay.Shop;
using RogueIslands.Gameplay.View.Boosters;
using RogueIslands.Gameplay.View.DeckBuilding;
using RogueIslands.Gameplay.View.Feedbacks;
using RogueIslands.Localization;
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
        [SerializeField] private BuildingCardView _buildingCardView;
        [SerializeField] private Transform _cardParent;
        [SerializeField] private Button _continue, _reroll;
        [SerializeField] private TextMeshProUGUI _rerollText;
        [SerializeField] private PopupOpeningFeedback _openingFeedback;

        private static ShopState Shop => GameManager.Instance.State.Shop;

        private void Awake()
        {
            _continue.onClick.AddListener(OnContinueClicked);
            _reroll.onClick.AddListener(OnRerollClicked);

            PopulateShop();
        }

        private void Start()
        {
            _openingFeedback.PlayOpening();
        }

        private void PopulateShop()
        {
            _cardParent.DestroyChildren();

            UpdateRerollCost();

            for (var i = 0; i < Shop.CardCount; i++)
            {
                var shopIndex = i;

                var item = Instantiate(_itemPrefab, _cardParent, false);

                switch (Shop.ItemsForSale[i])
                {
                    case BoosterCard booster:
                        InstantiateBoosterCard(item, booster);
                        break;
                    case Consumable consumable:
                        InstantiateConsumableCard(item, consumable);
                        break;
                    case Building building:
                        InstantiateBuildingCard(item, building);
                        break;
                }


                item.SetPrice(StaticResolver.Resolve<ILocalization>()
                    .Get("Buy-Price-Button", Shop.ItemsForSale[i].BuyPrice));

                item.BuyClicked += () =>
                {
                    var itemToBuy = Shop.ItemsForSale[shopIndex];
                    if (GameManager.Instance.State.Money < itemToBuy.BuyPrice)
                        return;

                    if (StaticResolver.Resolve<ShopPurchaseController>().PurchaseItemAtShop(shopIndex))
                    {
                        GameUI.Instance.RefreshMoney();

                        if (itemToBuy is IBooster)
                        {
                            var boosterCard = (BoosterView)GameManager.Instance.GetBooster(GameManager.Instance.State.Boosters.Last());
                            boosterCard.transform.position = item.transform.position;
                            boosterCard.BringToFront(item);
                            ResetOrderOfBoughtBooster(boosterCard);
                        }
                        
                        Destroy(item.gameObject);
                    }
                };

                item.GetComponent<DescriptionBoxSpawner>().Initialize((IDescribableItem)Shop.ItemsForSale[i]);
            }
        }

        private async void ResetOrderOfBoughtBooster(BoosterView booster)
        {
            await UniTask.WaitForSeconds(2);
            booster.ResetOrder();
        }

        private void InstantiateBoosterCard(ShopItem item, BoosterCard booster)
        {
            var card = Instantiate(_boosterPrefab, item.transform);
            card.Initialize(booster);
            Destroy(card.GetComponent<BoosterView>());
            Destroy(card.GetComponent<BoosterCardView>());
            Destroy(card.GetComponent<DescriptionBoxSpawner>());
        }

        private void UpdateRerollCost()
        {
            _rerollText.text = StaticResolver.Resolve<ILocalization>().Get("Reroll-Button", Shop.CurrentRerollCost);
        }

        private void OnRerollClicked()
        {
            if (GameManager.Instance.State.Money < Shop.CurrentRerollCost)
                return;

            StaticResolver.Resolve<ShopRerollController>().RerollShop();

            PopulateShop();
        }

        private async void OnContinueClicked()
        {
            await _openingFeedback.PlayClosing();
            StaticResolver.Resolve<RoundController>().StartRound();
            Destroy(gameObject);
        }

        private void InstantiateConsumableCard(ShopItem item, Consumable consumable)
        {
            var card = Instantiate(_consumableCardView, item.transform);
            card.Initialize(consumable);
            Destroy(card.GetComponent<ConsumableCardView>());
            Destroy(card.GetComponent<DescriptionBoxSpawner>());
        }

        private void InstantiateBuildingCard(ShopItem item, Building building)
        {
            var card = Instantiate(_buildingCardView, item.transform);
            card.Initialize(building);
            Destroy(card.GetComponent<BuildingCardView>());
            Destroy(card.GetComponent<DescriptionBoxSpawner>());
        }
    }
}