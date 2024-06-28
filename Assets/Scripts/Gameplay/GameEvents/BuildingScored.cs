using System.Collections.Generic;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.GameEvents
{
    public class BuildingScored : BuildingEvent
    {
        public List<Building> Cluster { get; set; }
    }
}