using RogueIslands.Gameplay.Boosters.Actions;

namespace RogueIslands.Gameplay.Boosters
{
    public interface IBooster : IDescribableItem, INamedItem
    {
        BoosterInstanceId Id { get; set; }
        GameAction EventAction { get; set; }
    }
}