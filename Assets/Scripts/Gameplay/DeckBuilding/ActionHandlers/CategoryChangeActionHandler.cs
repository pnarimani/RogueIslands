using System.Collections.Generic;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.DeckBuilding.Actions;

namespace RogueIslands.Gameplay.DeckBuilding.ActionHandlers
{
    public class CategoryChangeActionHandler : DeckActionHandler<CategoryChangeDeckAction>
    {
        protected override void Execute(CategoryChangeDeckAction action, IReadOnlyList<Building> selectedBuildings)
        {
            foreach (var building in selectedBuildings)
            {
                building.Category = action.TargetCategory;
                building.PrefabAddress = PrefabAddressProvider.GetPrefabAddress(building);
            }
        }
    }
}