using UnityEngine.Assertions;

namespace RogueIslands.Gameplay
{
    public static class Validation
    {
        public static void Validate(this GameState state)
        {
            foreach (var booster in state.Boosters) 
                Assert.IsFalse(booster.Id.IsDefault());
            
            foreach (var booster in state.AvailableBoosters) 
                Assert.IsTrue(booster.Id.IsDefault());
            
            foreach (var building in state.Buildings.Deck)
                Assert.IsFalse(building.Id.IsDefault());
            
            foreach (var building in state.Buildings.All)
                Assert.IsTrue(building.Id.IsDefault());
            
            Assert.IsNotNull(state.Shop);

            Assert.IsNotNull(state.Shop.ItemsForSale);
            Assert.AreEqual(state.Shop.CardCount, state.Shop.ItemsForSale.Length);

            Assert.IsNotNull(state.AllRequiredScores);
            Assert.AreEqual(GameState.RoundsPerAct * GameState.TotalActs, state.AllRequiredScores.Length);
            Assert.IsTrue(state.GetCurrentRequiredScore() > 0);
        }
    }
}