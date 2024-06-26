using System;
using RogueIslands.Boosters;
using RogueIslands.DeckBuilding;

namespace RogueIslands
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

            state.Money -= state.Shop.CurrentRerollCost;
            state.Shop.CurrentRerollCost =
                (int)MathF.Ceiling(state.Shop.CurrentRerollCost * state.Shop.RerollIncreaseRate);

            PopulateItemSlots(state);

            view.GetUI().RefreshAll();
        }

        private static void PopulateItemSlots(GameState state)
        {
            ref var selectionRand = ref state.Shop.SelectionRandom[state.Act];
            for (var i = 0; i < state.Shop.CardCount; i++)
            {
                var booster = selectionRand.NextInt(0, 2) == 1;
                if (booster)
                {
                    ref var rand = ref state.Shop.BoosterSpawn[state.Act];
                    state.Shop.ItemsForSale[i] = state.AvailableBoosters.SelectRandom(ref rand);
                }
                else
                {
                    ref var rand = ref state.Shop.CardPackSpawn[state.Act];
                    state.Shop.ItemsForSale[i] = state.DeckBuilding.AllConsumables.SelectRandom(ref rand);
                }
            }
        }

        public static void PurchaseItemAtShop(this GameState state, IGameView view, int index)
        {
            var item = state.Shop.ItemsForSale[index];
            if (state.Money < item.BuyPrice)
                throw new InvalidOperationException();

            var success = item switch
            {
                BoosterCard booster => StaticResolver.Resolve<BoosterManagement>().TryAddBooster(booster),
                Consumable consumable => view.GetDeckBuildingView().TryShowPopupForConsumable(consumable),
                _ => false,
            };

            if (success)
            {
                state.Money -= item.BuyPrice;
                state.Shop.ItemsForSale[index] = null;
            }
        }
    }
}