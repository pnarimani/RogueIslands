using RogueIslands.Gameplay.Rand;

namespace RogueIslands.Gameplay
{
    public class ShopState
    {
        public int StartingRerollCost = 5;
        public int CurrentRerollCost = 5;
        public int CardCount = 4;
        public float ConsumableSpawnChance = 0.25f;
        public float BuildingSpawnChance = 0.05f;
        public IPurchasableItem[] ItemsForSale;
        public RogueRandom SelectionRandom { get; set; }
        public RogueRandom BoosterSpawn { get; set; }
        public RogueRandom BuildingSpawn { get; set; }
        public RogueRandom DeduplicationRandom { get; set; }
        public RogueRandom CardPackSpawn { get; set; }
    }
}