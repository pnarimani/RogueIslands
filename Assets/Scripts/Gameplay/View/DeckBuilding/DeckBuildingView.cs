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
            var selector = _windowOpener.Open<BuildingSelectorPopup>();
            selector.BuildingsSelected += buildings =>
            {
                StaticResolver.Resolve<DeckBuildingController>().ExecuteConsumable(consumable, buildings);
            };
            selector.Show(consumable);
        }
    }
}