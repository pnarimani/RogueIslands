using RogueIslands.Gameplay.Rand;

namespace RogueIslands.Gameplay
{
    public class ShopState
    {
        public int StartingRerollCost = 5;
        public int CurrentRerollCost = 5;
        public int CardCount = 2;
        public float ConsumableSpawnChance = 0.25f;
        public IPurchasableItem[] ItemsForSale;
        public RogueRandom SelectionRandom;
        public RogueRandom BoosterSpawn;
        public RogueRandom DeduplicationRandom;
        public RogueRandom CardPackSpawn;
    }
}