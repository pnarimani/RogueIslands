using RogueIslands.Boosters.Actions;
using RogueIslands.Boosters.Descriptions;

namespace RogueIslands.Boosters
{
    public class BoosterCard : IPurchasableItem, IBooster
    {
        public BoosterInstanceId Id { get; set; }
        public string Name { get; set; }
        public IDescriptionProvider Description { get; set; }
        public int BuyPrice { get; set; }
        public int SellPrice { get; set; }
        public GameAction BuyAction { get; set; }
        public GameAction SellAction { get; set; }
        public GameAction EventAction { get; set; }
    }
}