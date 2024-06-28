using System.Collections.Generic;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.DeckBuilding.Actions;
using RogueIslands.Serialization;

namespace RogueIslands.Gameplay.DeckBuilding.ActionHandlers
{
    public class MirrorActionHandler : DeckActionHandler<MirrorDeckAction>
    {
        private readonly ICloner _cloner;
        private readonly GameState _state;

        public MirrorActionHandler(ICloner cloner, GameState state)
        {
            _state = state;
            _cloner = cloner;
        }
        
        protected override void Execute(MirrorDeckAction action, IReadOnlyList<Building> selectedBuildings)
        {
            var previousId = selectedBuildings[0].Id;
            var toReplaceIndex = _state.Buildings.Deck.IndexOf(selectedBuildings[0]);
            var newBuilding = _cloner.Clone(selectedBuildings[1]);
            newBuilding.Id = previousId;
            _state.Buildings.Deck[toReplaceIndex] = newBuilding;
        }
    }
}