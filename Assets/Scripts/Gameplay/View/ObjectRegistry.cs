using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RogueIslands.Diagnostics;
using RogueIslands.Gameplay.View.Boosters;
using UnityEngine;

namespace RogueIslands.Gameplay.View
{
    public class ObjectRegistry
    {
        private static int _cacheFrame = -1;

        private static readonly List<BuildingView> _cachedBuildings = new();
        private static readonly List<BoosterView> _cachedBoosters = new();
        private static readonly List<BuildingCardView> _cachedBuildingCards = new();

        public static IEnumerable<BuildingView> GetBuildings()
        {
            using var profiler = ProfilerScope.Begin();

            if (Time.frameCount != _cacheFrame)
            {
                RebuildCache();
                _cacheFrame = Time.frameCount;
            }

            return _cachedBuildings;
        }

        private static void RebuildCache()
        {
            _cachedBuildings.Clear();
            _cachedBoosters.Clear();
            _cachedBuildingCards.Clear();

            _cachedBuildings.AddRange(Object.FindObjectsByType<BuildingView>(FindObjectsSortMode.None)
                .Where(b => !b.IsPreview));
            _cachedBoosters.AddRange(Object.FindObjectsByType<BoosterView>(FindObjectsSortMode.None));
            _cachedBuildingCards.AddRange(Object.FindObjectsByType<BuildingCardView>(FindObjectsSortMode.None));
        }

        public static IReadOnlyList<BoosterView> GetBoosters()
        {
            using var profiler = ProfilerScope.Begin();

            if (Time.frameCount != _cacheFrame)
            {
                RebuildCache();
                _cacheFrame = Time.frameCount;
            }

            return _cachedBoosters;
        }

        public static IReadOnlyList<BuildingCardView> GetBuildingCards()
        {
            using var profiler = ProfilerScope.Begin();
            
            if (Time.frameCount != _cacheFrame)
            {
                RebuildCache();
                _cacheFrame = Time.frameCount;
            }

            return _cachedBuildingCards;
        }
    }
}