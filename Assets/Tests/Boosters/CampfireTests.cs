using System.Linq;
using NSubstitute;
using NUnit.Framework;
using RogueIslands.Boosters;
using RogueIslands.Boosters.Actions;
using RogueIslands.GameEvents;

namespace RogueIslands.Tests.Boosters
{
    public class CampfireTests
    {
        [Test]
        public void CampfireTestsSimplePasses()
        {
            var campfire = BoosterList.Get().First(x => x.Name == "Campfire");
            var state = new GameState();
            state.Boosters.Add(campfire);

            state.ExecuteEvent(Substitute.For<IGameView>(), new BoosterSold());

            Assert.AreEqual(1.5, campfire.GetEventAction<ScoringAction>().XMult);
        }
    }
}