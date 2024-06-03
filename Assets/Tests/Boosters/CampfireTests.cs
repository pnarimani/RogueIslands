using System.Linq;
using NSubstitute;
using NUnit.Framework;
using RogueIslands;
using RogueIslands.Boosters;

namespace Tests.Boosters
{
    public class CampfireTests
    {
        [Test]
        public void CampfireTestsSimplePasses()
        {
            var campfire = BoosterList.Get().First(x => x.Name == "Campfire");
            var state = new GameState();
            state.Boosters.Add(campfire);

            state.CurrentEvent = "BoosterSold";
            state.ExecuteAll(Substitute.For<IGameView>());

            Assert.AreEqual(1.5, campfire.GetEventAction<ScoringAction>().XMult);
        }
    }
}
