namespace RogueIslands
{
    public interface IPurchasableItem
    {
        int BuyPrice { get; set; }
        int SellPrice { get; set; }
    }
}