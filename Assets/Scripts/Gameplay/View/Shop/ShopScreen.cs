using System.Linq;
using Cysharp.Threading.Tasks;
using Flexalon;
using RogueIslands.Autofac;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.Shop;
using RogueIslands.Gameplay.View.Boosters;
using RogueIslands.Gameplay.View.Descriptions;
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
        [SerializeField] private BuildingCardView _buildingCardView;
        [SerializeField] private Transform _cardParent, _buildingCardParent;
        [SerializeField] private Button _continue, _reroll, _rerollBuildings;
        [SerializeField] private TextMeshProUGUI _rerollText, _rerollBuildingsText;
        [SerializeField] private PopupOpeningFeedback _openingFeedback;

        private ILocalization _localization;

        private static ShopState Shop => GameManager.Instance.State.Shop;

        private void Awake()
        {
            _continue.onClick.AddListener(OnContinueClicked);
            _reroll.onClick.AddListener(OnRerollClicked);
            _rerollBuildings.onClick.AddListener(OnRerollBuildingsClicked);
        }

        private void Start()
        {
            _localization = StaticResolver.Resolve<ILocalization>();
            _openingFeedback.PlayOpening();
            
            GameUI.Instance.RefreshScores();
            
            PopulateShop();
            PopulateBuildingShop();
        }

        private void PopulateShop()
        {
            _cardParent.DestroyChildren();

            UpdateRerollCost();

            foreach (var item in Shop.ItemsForSale)
            {
                SpawnItem(item, _cardParent);
            }
        }

        private void PopulateBuildingShop()
        {
            _buildingCardParent.DestroyChildren();
            
            UpdateRerollCost();
            
            foreach (var building in Shop.BuildingCards)
            {
                SpawnItem(building, _buildingCardParent);
            }
        }

        private void SpawnItem(IPurchasableItem shopItem, Transform parent)
        {
            var item = Instantiate(_itemPrefab, parent, false);
            item.transform.localPosition = Vector3.zero;

            if (shopItem == null)
            {
                item.ShowBuyButton(false);
                return;
            }

            switch (shopItem)
            {
                case BoosterCard booster:
                    InstantiateBoosterCard(item, booster);
                    break;
                case Building building:
                    InstantiateBuildingCard(item, building);
                    break;
            }

            item.SetPrice(shopItem.BuyPrice > 0 ? _localization.Get("Buy-Price-Button", shopItem.BuyPrice) : "Free");
            item.ShowBuyButton(true);

            item.BuyClicked += () =>
            {
                if (GameManager.Instance.State.Money < shopItem.BuyPrice)
                    return;

                if (StaticResolver.Resolve<ShopPurchaseController>().PurchaseItemAtShop(shopItem))
                {
                    GameUI.Instance.RefreshMoney();

                    if (shopItem is IBooster)
                    {
                        var boosterCard =
                            (BoosterView)GameManager.Instance.GetBooster(GameManager.Instance.State.Boosters
                                .Last().Id);
                        boosterCard.transform.position = item.transform.position;
                        boosterCard.BringToFront(item);
                        ResetOrderOfBoughtBooster(boosterCard);
                    }

                    item.ShowBuyButton(false);
                    Destroy(item.GetComponent<DescriptionBoxSpawner>());
                    Destroy(item.InnerObject);
                }
            };

            item.GetComponent<DescriptionBoxSpawner>().Initialize((IDescribableItem)shopItem);
        }

        private async void ResetOrderOfBoughtBooster(BoosterView booster)
        {
            await UniTask.WaitForSeconds(2);
            booster.ResetOrder();
        }
        
        private void InstantiateBoosterCard(ShopItem item, BoosterCard booster)
        {
            var card = Instantiate(_boosterPrefab, item.SpawnParent);
            card.Initialize(booster);
            Destroy(card.GetComponent<BoosterView>());
            Destroy(card.GetComponent<BoosterCardView>());
            Destroy(card.GetComponent<DescriptionBoxSpawner>());
            Destroy(card.GetComponent<InteractableObject>());

            item.InnerObject = card.gameObject;
        }
        
        private void InstantiateBuildingCard(ShopItem item, Building building)
        {
            var card = Instantiate(_buildingCardView, item.SpawnParent);
            card.Initialize(building);
            Destroy(card.GetComponent<BuildingCardView>());
            Destroy(card.GetComponent<DescriptionBoxSpawner>());
            Destroy(card.GetComponent<InteractableObject>());
            
            item.InnerObject = card.gameObject;
        }

        private void UpdateRerollCost()
        {
            _rerollText.text = _localization.Get("Reroll-Button", Shop.CurrentRerollCost);
            _rerollBuildingsText.text = _localization.Get("Reroll-Button", Shop.CurrentBuildingRerollCost);
        }

        private void OnRerollClicked()
        {
            if (GameManager.Instance.State.Money < Shop.CurrentRerollCost)
                return;

            StaticResolver.Resolve<ShopRerollController>().RerollShop();

            PopulateShop();
        }

        private void OnRerollBuildingsClicked()
        {
            if (GameManager.Instance.State.Money < Shop.CurrentBuildingRerollCost)
                return;

            StaticResolver.Resolve<ShopRerollController>().RerollBuildingSlots();

            PopulateBuildingShop();
        }

        private async void OnContinueClicked()
        {
            await _openingFeedback.PlayClosing();
            StaticResolver.Resolve<RoundController>().StartRound();
            Destroy(gameObject);
        }
    }
}