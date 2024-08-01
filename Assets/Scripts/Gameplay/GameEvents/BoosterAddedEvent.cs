using RogueIslands.Gameplay.Boosters;

namespace RogueIslands.Gameplay.GameEvents
{
    public class BoosterAddedEvent : IGameEvent
    {
        public IBooster Booster { get; set; }
    }
}