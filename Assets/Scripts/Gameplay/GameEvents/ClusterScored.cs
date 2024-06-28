using System.Collections.Generic;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.GameEvents
{
    public class ClusterScored : IGameEvent
    {
        public List<Building> Cluster;
    }
}