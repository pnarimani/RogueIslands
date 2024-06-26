using System;
using System.Collections.Generic;
using RogueIslands.Buildings;

namespace RogueIslands.DeckBuilding
{
    public class DeckBuildingController
    {
        private GameState _state;

        public DeckBuildingController(GameState state)
        {
            _state = state;
        }
        
        public void ExecuteConsumable(Consumable consumable, IReadOnlyList<Building> selectedBuildings)
        {
            switch (consumable.Action)
            {
                case Demolition:
                    foreach (var b in selectedBuildings) 
                        _state.Buildings.Deck.Remove(b);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}