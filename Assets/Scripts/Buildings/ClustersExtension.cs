using System.Collections.Generic;
using System.Linq;
using UnityEngine.PlayerLoop;

namespace RogueIslands.Buildings
{
    public static class ClustersExtension
    {
        public static List<List<Building>> GetClusters(this GameState state)
        {
            return state.Buildings.Deck
                .Where(b => b.IsPlacedDown())
                .GroupBy(b => b.ClusterId)
                .Select(b => b.ToList())
                .ToList();
        }
    }
}