using RogueIslands.Boosters;

namespace RogueIslands.GameEvents
{
    public class BoosterEvent : IGameEvent
    {
        public IBooster Booster { get; set; }
    }
}