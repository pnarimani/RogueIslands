using RogueIslands.Gameplay.DeckBuilding;
using RogueIslands.UISystem;

namespace RogueIslands.Gameplay.View.DeckBuilding
{
    public class DeckBuildingView : Singleton<DeckBuildingView>, IDeckBuildingView
    {
        private readonly IWindowOpener _windowOpener;

        public DeckBuildingView(IWindowOpener windowOpener)
        {
            _windowOpener = windowOpener;
        }
        
        public void ShowPopupForConsumable(Consumable consumable)
        {
            _windowOpener.Open<BuildingSelectorPopup>().Show(consumable);
        }
    }
}