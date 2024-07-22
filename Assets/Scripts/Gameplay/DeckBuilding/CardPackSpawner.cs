using System.Collections.Generic;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Serialization;

namespace RogueIslands.Gameplay.DeckBuilding
{
    public class CardPackSpawner
    {
        private readonly GameState _state;
        private readonly ICloner _cloner;

        public CardPackSpawner(GameState state, ICloner cloner)
        {
            _cloner = cloner;
            _state = state;
        }

        public List<Category> GetCategories()
        {
            var packs = new List<Category>();
            while (packs.Count < 3)
            {
                var category = Category.All.SelectRandom(_state.CardPackSelectionRandom.ForAct(_state.Act));
                if (!packs.Contains(category))
                    packs.Add(category);
            }

            return packs;
        }

        public List<Building> SelectCategory(Category category)
        {
            var buildingsInCategory = _state.Buildings.All.FindAll(b => b.Category == category);
            var random = _state.CardSelectionRandom.ForAct(_state.Act);
            var selectedBuildings = new List<Building>();

            for (var i = 0; i < _state.CardPerPack; i++)
            {
                var building = _cloner.Clone(buildingsInCategory.SelectRandom(random));
                building.Id = BuildingId.NewBuildingId();
                selectedBuildings.Add(building);
            }

            _state.Buildings.Deck.AddRange(selectedBuildings);
            
            return selectedBuildings;
        }
    }
}