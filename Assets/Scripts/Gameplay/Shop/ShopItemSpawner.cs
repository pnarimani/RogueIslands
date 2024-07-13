using RogueIslands.Gameplay.Boosters;
using UnityEngine;

namespace RogueIslands.Gameplay.Shop
{
    public class ShopItemSpawner
    {
        private readonly GameState _state;

        public ShopItemSpawner(GameState state)
            => _state = state;

        public void PopulateShop()
        {
            RepopulateSlots();

            _state.Shop.CurrentRerollCost = _state.Shop.StartingRerollCost;
        }

        public void RepopulateSlots()
        {
            _state.Shop.ItemsForSale = new IPurchasableItem[_state.Shop.CardCount];

            var selectionRand = _state.Shop.SelectionRandom.ForAct(_state.Act);
            for (var i = 0; i < _state.Shop.CardCount; i++)
            {
                var booster = selectionRand.NextFloat() > _state.Shop.ConsumableSpawnChance;
                IPurchasableItem item;
                if (booster)
                {
                    var randomForAct = _state.Shop.BoosterSpawn.ForAct(_state.Act);
                    item = _state.AvailableBoosters.SelectRandom(randomForAct);
                }
                else
                {
                    var randomForAct = _state.Shop.CardPackSpawn.ForAct(_state.Act);
                    item = _state.Consumables.AllConsumables.SelectRandom(randomForAct);
                }

                _state.Shop.ItemsForSale[i] = Deduplicate(item);
            }
        }

        private IPurchasableItem Deduplicate(IPurchasableItem item)
        {
            foreach (var existingItem in _state.Shop.ItemsForSale)
            {
                if (existingItem?.Name == item.Name)
                    return ReplaceItem(item);
            }
            
            foreach (var booster in _state.Boosters)
            {
                if (booster.Name == item.Name)
                    return ReplaceItem(item);
            }

            return item;
        }

        private IPurchasableItem ReplaceItem(IPurchasableItem item)
        {
            var dupeRandom = _state.Shop.DeduplicationRandom.ForAct(_state.Act);
            if (item is IBooster)
                item = _state.AvailableBoosters.SelectRandom(dupeRandom);
            else
                item = _state.Consumables.AllConsumables.SelectRandom(dupeRandom);
            return Deduplicate(item);
        }
    }
}