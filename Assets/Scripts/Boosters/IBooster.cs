using RogueIslands.Boosters.Actions;
using RogueIslands.Boosters.Descriptions;

namespace RogueIslands.Boosters
{
    public interface IBooster
    {
        BoosterInstanceId Id { get; set; }
        string Name { get; set; }
        IDescriptionProvider Description { get; set; }
        GameAction EventAction { get; set; }
    }
}