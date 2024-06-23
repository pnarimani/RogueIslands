using RogueIslands.Boosters.Actions;
using RogueIslands.Boosters.Descriptions;

namespace RogueIslands.Boosters
{
    public interface IBooster : IDescribableItem, INamedItem
    {
        BoosterInstanceId Id { get; set; }
        GameAction EventAction { get; set; }
    }
}