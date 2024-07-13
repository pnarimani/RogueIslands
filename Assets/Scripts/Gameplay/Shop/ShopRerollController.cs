using System;
using UnityEngine;

namespace RogueIslands.Gameplay.Shop
{
    public class ShopRerollController
    {
        private readonly GameState _state;
        private readonly IGameView _view;
        private readonly ShopItemSpawner _shopItemSpawner;

        public ShopRerollController(GameState state, IGameView view, ShopItemSpawner shopItemSpawner)
        {
            _shopItemSpawner = shopItemSpawner;
            _view = view;
            _state = state;
        }
        
        public void RerollShop()
        {
            if (_state.Money < _state.Shop.CurrentRerollCost)
                throw new InvalidOperationException();

            _state.Money -= Mathf.RoundToInt(_state.Shop.CurrentRerollCost);
            _state.Shop.CurrentRerollCost =
                (int)MathF.Ceiling(_state.Shop.CurrentRerollCost + 1);

            _shopItemSpawner.RepopulateSlots();

            _view.GetUI().RefreshAll();
        }
    }
}