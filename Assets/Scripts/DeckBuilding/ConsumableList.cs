using System.Collections.Generic;
using RogueIslands.Boosters.Descriptions;

namespace RogueIslands.DeckBuilding
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
                    Action = new Demolition(),
                },
            };
        }
    }
}