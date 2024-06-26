using Unity.Mathematics;

namespace RogueIslands
{
    public class ShopState
    {
        public int StartingRerollCost = 5;
        public int CurrentRerollCost = 5;
        public float RerollIncreaseRate = 1.5f;
        public int CardCount = 2;
        public IPurchasableItem[] ItemsForSale;
        public Random[] SelectionRandom;
        public Random[] BoosterSpawn;
        public Random[] BoosterAntiDuplicate;
        public Random[] CardPackSpawn;
    }
}