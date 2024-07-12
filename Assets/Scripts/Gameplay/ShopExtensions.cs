using System;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.DeckBuilding;
using UnityEngine;

namespace RogueIslands.Gameplay
{
    public static class ShopExtensions
    {
        public static void PopulateShop(this GameState state)
        {
            state.Shop.ItemsForSale = new IPurchasableItem[state.Shop.CardCount];

            PopulateItemSlots(state);

            state.Shop.CurrentRerollCost = state.Shop.StartingRerollCost;
        }

        public static void RerollShop(this GameState state, IGameView view)
        {
            if (state.Money < state.Shop.CurrentRerollCost)
                throw new InvalidOperationException();

            state.Money -= Mathf.RoundToInt(state.Shop.CurrentRerollCost);
            state.Shop.CurrentRerollCost =
                (int)MathF.Ceiling(state.Shop.CurrentRerollCost + 1);

            PopulateItemSlots(state);

            view.GetUI().RefreshAll();
        }

        private static void PopulateItemSlots(GameState state)
        {
            var selectionRand = state.Shop.SelectionRandom.ForAct(state.Act);
            for (var i = 0; i < state.Shop.CardCount; i++)
            {
                var booster = selectionRand.NextFloat() > state.Shop.ConsumableSpawnChance;
                if (booster)
                {
                    var randomForAct = state.Shop.BoosterSpawn.ForAct(state.Act);
                    state.Shop.ItemsForSale[i] = state.AvailableBoosters.SelectRandom(randomForAct);
                }
                else
                {
                    var randomForAct = state.Shop.CardPackSpawn.ForAct(state.Act);
                    state.Shop.ItemsForSale[i] = state.DeckBuilding.AllConsumables.SelectRandom(randomForAct);
                }
            }
        }

        public static void PurchaseItemAtShop(this GameState state, IGameView view, int index)
        {
            var item = state.Shop.ItemsForSale[index];
            if (state.Money < item.BuyPrice)
                throw new InvalidOperationException();

            bool success;
            switch (item)
            {
                case BoosterCard booster:
                    success = StaticResolver.Resolve<BoosterManagement>().TryAddBooster(booster);
                    break;
                case Consumable consumable:
                    view.GetDeckBuildingView().ShowPopupForConsumable(consumable);
                    success = true;
                    break;
                default:
                    success = false;
                    break;
            }

            if (success)
            {
                state.Money -= item.BuyPrice;
                state.Shop.ItemsForSale[index] = null;
            }
        }
    }
}