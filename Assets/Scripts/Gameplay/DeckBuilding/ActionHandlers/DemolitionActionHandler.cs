using System.Collections.Generic;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.DeckBuilding.Actions;

namespace RogueIslands.Gameplay.DeckBuilding.ActionHandlers
{
    public class DemolitionActionHandler : DeckActionHandler<DemolitionDeckAction>
    {
        private readonly GameState _state;

        public DemolitionActionHandler(GameState state)
        {
            _state = state;
        }
        
        protected override void Execute(DemolitionDeckAction action, IReadOnlyList<Building> selectedBuildings)
        {
            foreach (var b in selectedBuildings) 
                _state.Buildings.Deck.Remove(b);
        }
    }
}