using System;
using System.Linq;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.DeckBuilding;
using RogueIslands.Serialization;

namespace RogueIslands.Gameplay.Shop
{
    public class ShopPurchaseController
    {
        private readonly GameState _state;
        private readonly BoosterManagement _boosterManagement;
        private readonly IGameView _view;
        private readonly ICloner _cloner;

        public ShopPurchaseController(GameState state, BoosterManagement boosterManagement, IGameView view, ICloner cloner)
        {
            _cloner = cloner;
            _view = view;
            _boosterManagement = boosterManagement;
            _state = state;
        }

        public bool PurchaseItemAtShop(IPurchasableItem item)
        {
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
                case Building buildingBlueprint:
                    var building = _cloner.Clone(buildingBlueprint);
                    building.Id = BuildingId.NewBuildingId();
                    _state.Buildings.Deck.Add(building);
                    _view.GetUI().RefreshDeckText();
                    if (_state.BuildingsInHand.Contains(building))
                        _view.GetUI().ShowBuildingCard(building);
                    if (_state.DeckPeek.Contains(building))
                        _view.GetUI().ShowBuildingCardPeek(building);
                    success = true;
                    break;
                default:
                    success = false;
                    break;
            }

            if (success)
            {
                _state.Money -= item.BuyPrice;

                if (item is Building building)
                {
                    var index = Array.IndexOf(_state.Shop.BuildingCards, building);
                    _state.Shop.BuildingCards[index] = null;

                    _state.Shop.BuildingCardPrices[index]++;
                }
                else
                {
                    var index = Array.IndexOf(_state.Shop.ItemsForSale, item);
                    if (index >= 0)
                    {
                        _state.Shop.ItemsForSale[index] = null;
                    }
                }
            }

            return success;
        }
    }
}