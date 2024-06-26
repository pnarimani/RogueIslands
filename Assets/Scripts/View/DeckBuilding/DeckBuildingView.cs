using RogueIslands.DeckBuilding;
using UnityEngine;

namespace RogueIslands.View.DeckBuilding
{
    public class DeckBuildingView : SingletonMonoBehaviour<DeckBuildingView>, IDeckBuildingView
    {
        [SerializeField] private BuildingSelectorPopup _buildingSelectorPrefab;
        
        public bool TryShowPopupForConsumable(Consumable consumable)
        {
            switch (consumable.Action)
            {
                case Demolition:
                    var selector = Instantiate(_buildingSelectorPrefab);
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