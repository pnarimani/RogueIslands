using System;
using RogueIslands.Gameplay.GameEvents;

namespace RogueIslands.Gameplay.Shop
{
    public class ShopRerollController
    {
        private readonly GameState _state;
        private readonly IGameView _view;
        private readonly ShopItemSpawner _shopItemSpawner;
        private readonly IEventController _eventController;

        public ShopRerollController(GameState state, IGameView view, ShopItemSpawner shopItemSpawner,
            IEventController eventController)
        {
            _eventController = eventController;
            _shopItemSpawner = shopItemSpawner;
            _view = view;
            _state = state;
        }

        public void RerollShop()
        {
            if (_state.Money < _state.Shop.CurrentRerollCost)
                throw new InvalidOperationException();

            _state.Money -= _state.Shop.CurrentRerollCost;
            _state.Shop.CurrentRerollCost += 1;

            _shopItemSpawner.PopulateShop();

            _eventController.Execute(new ShopRerolledEvent());

            _view.GetUI().RefreshMoney();
        }

        public void RerollBuildingSlots()
        {
            if (_state.Money < _state.Shop.CurrentBuildingRerollCost)
                throw new InvalidOperationException();
            
            _state.Money -= _state.Shop.CurrentBuildingRerollCost;
            _state.Shop.CurrentBuildingRerollCost += 1;
            
            _shopItemSpawner.RepopulateBuildings();
            
            _eventController.Execute(new ShopRerolledEvent());
            
            _view.GetUI().RefreshMoney();
        }

        public void ResetRerollCosts()
        {
            _state.Shop.CurrentRerollCost = _state.Shop.StartingRerollCost;
            _state.Shop.CurrentBuildingRerollCost = _state.Shop.StartingBuildingRerollCost;
        }
    }
}