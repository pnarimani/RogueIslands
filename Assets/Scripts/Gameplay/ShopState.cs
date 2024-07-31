﻿using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.Rand;

namespace RogueIslands.Gameplay
{
    public class ShopState
    {
        public int StartingRerollCost = 5;
        public int CurrentRerollCost = 5;

        public int StartingBuildingRerollCost { get; set; } = 1;
        public int CurrentBuildingRerollCost { get; set; } = 1;
        
        public int CardCount = 3;
        public float ConsumableSpawnChance = 0.0f;
        public float BuildingSpawnChance = 0.0f;
        public IPurchasableItem[] ItemsForSale { get; set; }
        public Building[] BuildingCards { get; set; } = new Building[6];
        public int[] BuildingCardPrices { get; set; } = new int[6];
        public RogueRandom SelectionRandom { get; set; }
        public RogueRandom RarityRandom { get; set; }
        public RogueRandom BoosterSpawn { get; set; }
        public RogueRandom BuildingSpawn { get; set; }
        public RogueRandom DeduplicationRandom { get; set; }
        public RogueRandom CardPackSpawn { get; set; }

        public float CommonWeight = 70;
        public float UncommonWeight = 25;
        public float RareWeight = 5;
    }
}