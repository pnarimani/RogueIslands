namespace RogueIslands.Gameplay
{
    public interface IPurchasableItem : INamedItem
    {
        int BuyPrice { get; set; }
        int SellPrice { get; set; }
    }
}