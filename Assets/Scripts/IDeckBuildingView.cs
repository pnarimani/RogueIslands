using RogueIslands.DeckBuilding;

namespace RogueIslands
{
    public interface IDeckBuildingView
    {
        bool TryShowPopupForConsumable(Consumable consumable);
    }
}