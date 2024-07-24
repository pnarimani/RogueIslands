using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RogueIslands.Gameplay.View.Boosters;
using UnityEngine;

namespace RogueIslands.Gameplay.View
{
    public class ObjectRegistry
    {
        public static IEnumerable<BuildingView> GetBuildings()
        {
            return Object.FindObjectsByType<BuildingView>(FindObjectsSortMode.None).Where(b => !b.IsPreview);
        }

        public static IReadOnlyList<BoosterView> GetBoosters()
        {
            return Object.FindObjectsByType<BoosterView>(FindObjectsSortMode.None);
        }

        public static IReadOnlyList<BuildingCardView> GetBuildingCards()
        {
            return Object.FindObjectsByType<BuildingCardView>(FindObjectsSortMode.None);
        }

        public static IReadOnlyList<WorldBoosterView> GetWorldBoosters()
        {
            return Object.FindObjectsByType<WorldBoosterView>(FindObjectsSortMode.None);
        }

        public static List<Vector3> GetWorldBoosterSpawnPoints()
        {
            var all = Object.FindObjectsOfType<WorldBoosterSpawnPoint>()
                .Select(p => p.transform.position)
                .ToList();

            all.Sort((a, b) =>
            {
                if(a.x < b.x)
                    return -1;
                if(a.y < b.y)
                    return -1;
                if (a.z < b.z)
                    return -1;
                return 1;
            });

            return all;
        }
    }
}