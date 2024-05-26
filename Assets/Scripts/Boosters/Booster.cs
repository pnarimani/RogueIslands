using System.Collections.Generic;

namespace RogueIslands.Boosters
{
    public class Booster
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public GameAction BuyAction { get; set; }
        public GameAction SellAction { get; set; }
        public GameAction EventAction { get; set; }
    }
}