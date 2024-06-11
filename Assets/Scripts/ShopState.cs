using RogueIslands.Boosters;
using Unity.Mathematics;

namespace RogueIslands
{
    public class ShopState
    {
        public int StartingRerollCost { get; set; } = 5;
        public int CurrentRerollCost { get; set; } = 5;
        public float RerollIncreaseRate { get; set; } = 1.5f;
        public int CardCount { get; set; } = 2;
        public IPurchasableItem[] ItemsForSale { get; set; }
        
        public Random[] BoosterSpawn { get; set; }
        public Random[] BoosterAntiDuplicate { get; set; }
        public Random[] CardPackSpawn { get; set; }
    }
}