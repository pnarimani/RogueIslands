using UnityEngine;

namespace RogueIslands
{
    public class PlacedBuilding
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public int RemainingTriggers { get; set; }
        public Building Building { get; set; }
    }
}