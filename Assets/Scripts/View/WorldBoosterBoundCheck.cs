using RogueIslands.GameEvents;
using RogueIslands.View.Boosters;
using UnityEngine;

namespace RogueIslands.View
{
    public class WorldBoosterBoundCheck
    {
        public static void HighlightOverlappingWorldBoosters(Transform building)
        {
            var worldBoosters = Object.FindObjectsOfType<WorldBoosterView>();
            var bounds = building.GetCollisionBounds();
            foreach (var booster in worldBoosters)
            {
                var wbBounds = booster.transform.GetCollisionBounds();
                booster.WarnDeletion(bounds.Intersects(wbBounds));
            }
        }

        public static void HideAllDeletionWarnings()
        {
            var worldBoosters = Object.FindObjectsOfType<WorldBoosterView>();
            foreach (var booster in worldBoosters)
            {
                booster.WarnDeletion(false);
            }
        }
    }
}