using RogueIslands.Gameplay.DeckBuilding.Actions;

namespace RogueIslands.Gameplay.DeckBuilding
{
    public class Consumable : IPurchasableItem, IDescribableItem, INamedItem, IHasAlternateDescriptionTitle
    {
        public string Name { get; set; }
        public IDescriptionProvider Description { get; set; }
        public int BuyPrice { get; set; }
        public int SellPrice { get; set; }
        public DeckAction Action { get; set; }
        
        public string AlternateTitle => "Consumable";
    }
}