using System.Collections.Generic;
using RogueIslands.Buildings;

namespace RogueIslands.GameEvents
{
    public class BuildingScored : BuildingEvent
    {
        public List<Building> Cluster { get; set; }
    }
}