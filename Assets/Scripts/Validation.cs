using System.Linq;
using UnityEngine.Assertions;

namespace RogueIslands
{
    public static class Validation
    {
        public static void Validate(this GameState state)
        {
            foreach (var booster in state.Boosters) 
                Assert.IsFalse(booster.Id.IsDefault());
            
            foreach (var building in state.Islands.SelectMany(x => x))
                Assert.IsFalse(building.Id.IsDefault());
            
            foreach (var booster in state.AvailableBoosters) 
                Assert.IsTrue(booster.Id.IsDefault());
            
            foreach (var building in state.AvailableBuildings)
                Assert.IsTrue(building.Id.IsDefault());
            
            state.BuildingsInHand.ForEach(x => Assert.IsTrue(x.Id.IsDefault()));

            Assert.IsNotNull(state.Shop);

            Assert.IsNotNull(state.Shop.ItemsForSale);
            Assert.AreEqual(state.Shop.CardCount, state.Shop.ItemsForSale.Length);

            Assert.IsNotNull(state.AllRequiredScores);
            Assert.AreEqual(GameState.TotalRounds * GameState.TotalActs, state.AllRequiredScores.Length);
            Assert.IsTrue(state.RequiredScore > 0);
        }
    }
}