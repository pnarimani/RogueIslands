namespace RogueIslands.Boosters
{
    public class Booster
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int BuyPrice { get; set; }
        public int SellPrice { get; set; }
        public GameAction BuyAction { get; set; }
        public GameAction SellAction { get; set; }
        public GameAction EventAction { get; set; }
    }
}