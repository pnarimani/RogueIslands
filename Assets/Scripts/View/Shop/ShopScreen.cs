using RogueIslands.Boosters;
using RogueIslands.View.Boosters;
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
                var shopIndex = i;
                
                var item = Instantiate(_itemPrefab, _cardParent);

                switch (Shop.ItemsForSale[i])
                {
                    case BoosterCard booster:
                        Instantiate(_boosterPrefab, item.transform).Initialize(booster);
                        break;
                }
                

                item.SetPrice($"${Shop.ItemsForSale[i].BuyPrice}");
                item.BuyClicked += () =>
                {
                    if (GameManager.Instance.State.Money < Shop.ItemsForSale[shopIndex].BuyPrice)
                        return;

                    GameManager.Instance.State.PurchaseItemAtShop(GameManager.Instance, shopIndex);
                    
                    Destroy(item.gameObject);
                };
            }
        }

        private void OnRerollClicked()
        {
            if (GameManager.Instance.State.Money < Shop.CurrentRerollCost)
                return;
            
            GameManager.Instance.State.RerollShop();
            
            PopulateShop();
        }

        private void OnContinueClicked()
        {
            GameManager.Instance.State.StartWeek(GameManager.Instance);
            
            Destroy(gameObject);
        }
    }
}