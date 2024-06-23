using RogueIslands.Boosters.Descriptions;
using UnityEngine;

namespace RogueIslands
{
    public class Building : IDescribableItem
    {
        public BuildingInstanceId Id;
        public Vector3 Position;
        public Quaternion Rotation;
        public int RemainingTriggers;
        public string Name;
        public string PrefabAddress;
        public string IconAddress;
        public float Range;
        public Category Category;
        public ColorTag Color;
        public BuildingSize Size;
        public double Output;
        public double OutputUpgrade;
        public IDescriptionProvider Description { get; set; }
    }
}