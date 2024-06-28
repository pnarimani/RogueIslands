using RogueIslands.Gameplay.Boosters;

namespace RogueIslands.Gameplay.GameEvents
{
    public class BoosterAdded : IGameEvent
    {
        public IBooster Booster { get; set; }
    }
}