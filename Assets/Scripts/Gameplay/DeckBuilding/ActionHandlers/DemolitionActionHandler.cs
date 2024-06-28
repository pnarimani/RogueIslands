using System.Collections.Generic;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.DeckBuilding.ActionHandlers
{
    public class DemolitionActionHandler : DeckActionHandler<Demolition>
    {
        private readonly GameState _state;

        public DemolitionActionHandler(GameState state)
        {
            _state = state;
        }
        
        protected override void Execute(Demolition action, IReadOnlyList<Building> selectedBuildings)
        {
            foreach (var b in selectedBuildings) 
                _state.Buildings.Deck.Remove(b);
        }
    }
}