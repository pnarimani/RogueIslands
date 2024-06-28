using RogueIslands.Gameplay.DeckBuilding;

namespace RogueIslands.Gameplay
{
    public interface IDeckBuildingView
    {
        bool TryShowPopupForConsumable(Consumable consumable);
    }
}