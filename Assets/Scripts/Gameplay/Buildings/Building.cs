using System;
using UnityEngine;

namespace RogueIslands.Gameplay.Buildings
{
    public class Building : IDescribableItem, IPurchasableItem
    {
        public BuildingId Id;
        public Vector3 Position;
        public Quaternion Rotation;
        public int RemainingTriggers;
        public string PrefabAddress;
        public string IconAddress;
        public float Range;
        public Category Category;
        public ColorTag Color;
        public BuildingSize Size;
        public double Output;
        public double OutputUpgrade;
        public IDescriptionProvider Description { get; set; }

        public string Name { get; set; }
        public int BuyPrice { get; set; }
        public int SellPrice { get; set; }

        public bool IsPlacedDown(GameState state) 
            => state.Buildings.PlacedDownBuildings.Exists(b => b.Id == Id);
    }
}