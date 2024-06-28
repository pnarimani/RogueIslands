using System.Collections.Generic;
using RogueIslands.Gameplay.Boosters.Descriptions;
using RogueIslands.Gameplay.DeckBuilding.Actions;

namespace RogueIslands.Gameplay.DeckBuilding
{
    public static class ConsumableList
    {
        public static List<Consumable> Get()
        {
            return new List<Consumable>()
            {
                new()
                {
                    Name = "Demolition",
                    Description = new LiteralDescription("Destroy up to 2 buildings"),
                    BuyPrice = 2,
                    SellPrice = 1,
                    Action = new DemolitionDeckAction()
                    {
                        MinCardsRequired = 0,
                        MaxCardsRequired = 2,
                    },
                },
                new()
                {
                    Name = "Mirror",
                    Description = new LiteralDescription("Select 2 buildings. Turns the left building into the right building"),
                    BuyPrice = 2,
                    SellPrice = 1,
                    Action = new MirrorDeckAction()
                    {
                        MinCardsRequired = 2,
                        MaxCardsRequired = 2,
                    },
                },
            };
        }
    }
}