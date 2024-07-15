﻿using System.Collections.Generic;
using System.Linq;

namespace RogueIslands.Gameplay.Buildings
{
    public static class ClustersExtension
    {
        public static List<List<Building>> GetClusters(this GameState state)
        {
            return state.PlacedDownBuildings
                .GroupBy(b => b.ClusterId)
                .Select(b => b.ToList())
                .ToList();
        }
    }
}