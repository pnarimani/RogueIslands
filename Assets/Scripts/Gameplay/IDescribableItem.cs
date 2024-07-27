using RogueIslands.Gameplay.Descriptions;

namespace RogueIslands.Gameplay
{
    public interface IDescribableItem
    {
        DescriptionData Description { get; }
    }
}