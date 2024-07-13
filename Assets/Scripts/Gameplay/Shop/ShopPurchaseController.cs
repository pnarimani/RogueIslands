using System;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.DeckBuilding;

namespace RogueIslands.Gameplay.Shop
{
    public class ShopPurchaseController
    {
        private readonly GameState _state;
        private readonly BoosterManagement _boosterManagement;
        private readonly IGameView _view;

        public ShopPurchaseController(GameState state, BoosterManagement boosterManagement, IGameView view)
        {
            _view = view;
            _boosterManagement = boosterManagement;
            _state = state;
        }
        
        public bool PurchaseItemAtShop(int index)
        {
            var item = _state.Shop.ItemsForSale[index];
            if (_state.Money < item.BuyPrice)
                return false;

            bool success;
            switch (item)
            {
                case BoosterCard booster:
                    success = _boosterManagement.TryAddBooster(booster);
                    break;
                case Consumable consumable:
                    _view.GetDeckBuildingView().ShowPopupForConsumable(consumable);
                    success = true;
                    break;
                default:
                    success = false;
                    break;
            }

            if (success)
            {
                _state.Money -= item.BuyPrice;
                _state.Shop.ItemsForSale[index] = null;
            }

            return success;
        }
    }
}