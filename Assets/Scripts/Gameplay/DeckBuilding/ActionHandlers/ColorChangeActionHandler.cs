using System.Collections.Generic;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.DeckBuilding.Actions;

namespace RogueIslands.Gameplay.DeckBuilding.ActionHandlers
{
    public class ColorChangeActionHandler : DeckActionHandler<ColorChangeDeckAction>
    {
        protected override void Execute(ColorChangeDeckAction action, IReadOnlyList<Building> selectedBuildings)
        {
            foreach (var building in selectedBuildings)
            {
                building.Color = action.TargetColor;
                building.PrefabAddress = PrefabAddressProvider.GetPrefabAddress(building);
            }
        }
    }
}