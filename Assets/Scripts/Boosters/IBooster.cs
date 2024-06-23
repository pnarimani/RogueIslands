using RogueIslands.Boosters.Actions;
using RogueIslands.Boosters.Descriptions;

namespace RogueIslands.Boosters
{
    public interface IBooster : IDescribableItem
    {
        BoosterInstanceId Id { get; set; }
        string Name { get; set; }
        GameAction EventAction { get; set; }
    }
}