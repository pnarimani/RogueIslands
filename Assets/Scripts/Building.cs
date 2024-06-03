using UnityEngine;

namespace RogueIslands
{
    public class Building
    {
        public BuildingInstanceId Id { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public int RemainingTriggers { get; set; }
        public string Name { get; set;}
        public string Description { get; set; }
        public string PrefabAddress { get; set; }
        public float Range { get; set; }
        public Category Category { get; set; }
        public ColorTag Color { get; set; }
        public int EnergyCost { get; set; }
        public double Output { get; set; }
        
        public double OutputUpgrade { get; set; }
    }
}