using System.Collections;
using System.Collections.Generic;
using RogueIslands.Gameplay.View.Boosters;
using UnityEngine;

namespace RogueIslands.Gameplay.View
{
    public class ObjectRegistry
    {
        public static IReadOnlyList<BuildingView> GetBuildings()
        {
            return Object.FindObjectsByType<BuildingView>(FindObjectsSortMode.None);
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
    }
}