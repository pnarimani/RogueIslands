using RogueIslands.Boosters;

namespace RogueIslands.GameEvents
{
    public class BoosterAdded : IGameEvent
    {
        public IBooster Booster { get; set; }
    }
}