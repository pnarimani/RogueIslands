using System;
using System.Collections.Generic;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.DeckBuilding.ActionHandlers;

namespace RogueIslands.Gameplay.DeckBuilding
{
    public class DeckBuildingController
    {
        private readonly IReadOnlyList<DeckActionHandler> _handlers;

        public DeckBuildingController(IReadOnlyList<DeckActionHandler> handlers)
        {
            _handlers = handlers;
        }
        
        public void ExecuteConsumable(Consumable consumable, IReadOnlyList<Building> selectedBuildings)
        {
            foreach (var handler in _handlers)
            {
                if (handler.CanHandle(consumable.Action))
                {
                    handler.Execute(consumable.Action, selectedBuildings);
                    break;
                }
            }
        }
    }
}