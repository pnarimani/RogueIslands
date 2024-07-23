using System;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.DeckBuilding;
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
            _state.Shop.ItemsForSale = new IPurchasableItem[_state.Shop.CardCount];

            var selectionRand = _state.Shop.SelectionRandom.ForAct(_state.Act);
            for (var i = 0; i < _state.Shop.CardCount; i++)
            {
                IPurchasableItem item;
                if (selectionRand.NextFloat() < _state.Shop.BuildingSpawnChance)
                {
                    var random = _state.Shop.BuildingSpawn.ForAct(_state.Act);
                    item = DefaultBuildingsList.Get().SelectRandom(random);
                    item.BuyPrice = 2;
                }
                else
                {
                    var booster = selectionRand.NextFloat() > _state.Shop.ConsumableSpawnChance;
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

                    item = Deduplicate(item, 0);
                }

                _state.Shop.ItemsForSale[i] = item;
            }
        }

        public void RepopulateBuildings()
        {
            var random = _state.Shop.BuildingSpawn.ForAct(_state.Act);
            var cards = _state.Shop.BuildingCards;
            for (var i = 0; i < cards.Length; i++)
            {
                var item = DeduplicateBuilding(_state.Buildings.All.SelectRandom(random), 0);
                item.BuyPrice = _state.Shop.BuildingCardPrices[i];
                cards[i] = item;
            }
        }
        
        private Building DeduplicateBuilding(Building item, int depth)
        {
            if (depth >= 10)
                return item;
            
            var random = _state.Shop.BuildingSpawn.ForAct(_state.Act);
            
            foreach (var existingItem in _state.Shop.BuildingCards)
            {
                if (existingItem?.Name == item.Name)
                    return DeduplicateBuilding(_state.Buildings.All.SelectRandom(random), depth + 1);
            }

            return item;
        }

        private IPurchasableItem Deduplicate(IPurchasableItem item, int depth)
        {
            if (depth >= 10)
                return item;
            
            foreach (var existingItem in _state.Shop.ItemsForSale)
            {
                if (existingItem?.Name == item.Name)
                    return ReplaceItem(item, depth + 1);
            }

            foreach (var booster in _state.Boosters)
            {
                if (booster.Name == item.Name)
                    return ReplaceItem(item, depth + 1);
            }

            return item;
        }

        private IPurchasableItem ReplaceItem(IPurchasableItem item, int depth)
        {
            var dupeRandom = _state.Shop.DeduplicationRandom.ForAct(_state.Act);
            if (item is IBooster)
                item = _state.AvailableBoosters.SelectRandom(dupeRandom);
            else if (item is Consumable)
                item = _state.Consumables.AllConsumables.SelectRandom(dupeRandom);
            else
                throw new InvalidOperationException();
            return Deduplicate(item, depth);
        }
    }
}