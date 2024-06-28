using RogueIslands.Gameplay.DeckBuilding;
using RogueIslands.UISystem;
using UnityEngine;

namespace RogueIslands.Gameplay.View.DeckBuilding
{
    public class DeckBuildingView : Singleton<DeckBuildingView>, IDeckBuildingView
    {
        private IWindowOpener _windowOpener;

        public DeckBuildingView(IWindowOpener windowOpener)
        {
            _windowOpener = windowOpener;
        }
        
        public bool TryShowPopupForConsumable(Consumable consumable)
        {
            switch (consumable.Action)
            {
                case Demolition:
                    var selector = _windowOpener.Open<BuildingSelectorPopup>();
                    selector.BuildingsSelected += buildings =>
                    {
                        StaticResolver.Resolve<DeckBuildingController>().ExecuteConsumable(consumable, buildings);
                    };
                    selector.Show(consumable);
                    selector.Select(2);
                    return true;
                default:
                    return false;
            }
        }
    }
}