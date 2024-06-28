using RogueIslands.Gameplay.Boosters;

namespace RogueIslands.Gameplay.GameEvents
{
    public class BoosterEvent : IGameEvent
    {
        public IBooster Booster { get; set; }
    }
}