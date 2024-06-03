using System;
using RogueIslands.Boosters;

namespace RogueIslands
{
    public static class ShopExtensions
    {
        public static void PopulateShop(this GameState state)
        {
            state.Shop.BoostersForSale = new Booster[state.Shop.CardCount];
            
            PopulateBoosterSlots(state);

            state.Shop.CurrentRerollCost = state.Shop.StartingRerollCost;
        }

        public static void RerollShop(this GameState state)
        {
            if (state.Money < state.Shop.CurrentRerollCost)
                throw new InvalidOperationException();

            state.Money -= state.Shop.CurrentRerollCost;
            state.Shop.CurrentRerollCost = (int) MathF.Ceiling(state.Shop.CurrentRerollCost * state.Shop.RerollIncreaseRate);
            
            PopulateBoosterSlots(state);
        }

        private static void PopulateBoosterSlots(GameState state)
        {
            ref var rand = ref state.Shop.BoosterSpawn[state.Month];
            for (var i = 0; i < state.Shop.CardCount; i++)
            {
                var index = rand.NextInt(state.AvailableBoosters.Count);
                state.Shop.BoostersForSale[i] = state.AvailableBoosters[index];
            }
        }

        public static void PurchaseItemAtShop(this GameState state, IGameView view, int index)
        {
            var booster = state.Shop.BoostersForSale[index];
            if (state.Money < booster.BuyPrice)
                throw new InvalidOperationException();

            state.Money -= booster.BuyPrice;
            state.AddBooster(view, booster);
            state.Shop.BoostersForSale[index] = null;
        }
    }
}