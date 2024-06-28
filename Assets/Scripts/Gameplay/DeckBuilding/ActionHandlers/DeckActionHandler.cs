using System.Collections.Generic;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.DeckBuilding.Actions;

namespace RogueIslands.Gameplay.DeckBuilding.ActionHandlers
{
    public abstract class DeckActionHandler
    {
        public abstract bool CanHandle(DeckAction action);
        public abstract void Execute(DeckAction action, IReadOnlyList<Building> selectedBuildings);
    }
    
    public abstract class DeckActionHandler<T> : DeckActionHandler where T : DeckAction
    {
        public sealed override bool CanHandle(DeckAction action)
        {
            return action is T;
        }

        public sealed override void Execute(DeckAction action, IReadOnlyList<Building> selectedBuildings)
        {
            Execute((T) action, selectedBuildings);
        }

        protected abstract void Execute(T action, IReadOnlyList<Building> selectedBuildings);
    }
}