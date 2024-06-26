using System.Collections.Generic;
using RogueIslands.Buildings;

namespace RogueIslands.GameEvents
{
    public class ClusterScored : IGameEvent
    {
        public List<Building> Cluster;
    }
}