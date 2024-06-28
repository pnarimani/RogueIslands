using RogueIslands.Gameplay.View.Boosters;
using UnityEngine;

namespace RogueIslands.Gameplay.View
{
    public class WorldBoosterBoundCheck
    {
        public static void HighlightOverlappingWorldBoosters(Transform building)
        {
            var worldBoosters = ObjectRegistry.GetWorldBoosters();
            var bounds = building.GetCollisionBounds();
            foreach (var booster in worldBoosters)
            {
                var wbBounds = booster.transform.GetCollisionBounds();
                booster.WarnDeletion(bounds.Intersects(wbBounds));
            }
        }

        public static void HideAllDeletionWarnings()
        {
            var worldBoosters = ObjectRegistry.GetWorldBoosters();
            foreach (var booster in worldBoosters)
            {
                booster.WarnDeletion(false);
            }
        }
    }
}