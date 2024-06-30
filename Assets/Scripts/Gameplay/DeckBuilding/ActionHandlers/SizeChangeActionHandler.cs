using System;
using System.Collections.Generic;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.DeckBuilding.Actions;

namespace RogueIslands.Gameplay.DeckBuilding.ActionHandlers
{
    public class SizeChangeActionHandler : DeckActionHandler<SizeChangeDeckAction>
    {
        protected override void Execute(SizeChangeDeckAction action, IReadOnlyList<Building> selectedBuildings)
        {
            foreach (var building in selectedBuildings)
            {
                building.Size = building.Size switch
                {
                    BuildingSize.Small => BuildingSize.Medium,
                    BuildingSize.Medium => BuildingSize.Big,
                    BuildingSize.Big => BuildingSize.Small,
                    _ => throw new ArgumentOutOfRangeException(),
                };
                
                building.PrefabAddress = BuildingPrefabAddressProvider.GetPrefabAddress(building);
            }
        }
    }
}