using RogueIslands.Gameplay.Boosters.Actions;
using RogueIslands.Gameplay.Descriptions;

namespace RogueIslands.Gameplay.Boosters
{
    public class BoosterCard : IPurchasableItem, IBooster
    {
        public BoosterInstanceId Id { get; set; }
        public string Name { get; set; }
        public DescriptionData Description { get; set; }
        public int BuyPrice { get; set; }
        public int SellPrice { get; set; }
        public GameAction BuyAction { get; set; }
        public GameAction SellAction { get; set; }
        public GameAction EventAction { get; set; }
    }
}