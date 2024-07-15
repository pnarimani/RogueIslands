using System.Collections.Generic;
using RogueIslands.Gameplay.View.Boosters;
using UnityEngine;

namespace RogueIslands.Gameplay.View
{
    public class WorldBoosterBoundCheck
    {
        private static readonly Collider[] _colliderBuffer = new Collider[1000];
        private static readonly List<WorldBoosterView> _overlappingBoosters = new();
        
        public static void HighlightOverlappingWorldBoosters(Transform building)
        {
            GetOverlappingWorldBoosters(building);

            foreach (var wb in ObjectRegistry.GetWorldBoosters())
            {
                wb.WarnDeletion(_overlappingBoosters.Contains(wb));
            }
        }

        public static IReadOnlyList<WorldBoosterView> GetOverlappingWorldBoosters(Transform building)
        {
            var bounds = building.GetBounds();
            var count = Physics.OverlapBoxNonAlloc(bounds.center, bounds.extents, _colliderBuffer, building.rotation, LayerMask.GetMask("WorldBooster"));
            _overlappingBoosters.Clear();
            for (var i = 0; i < count; i++) 
                _overlappingBoosters.Add(_colliderBuffer[i].GetComponentInParent<WorldBoosterView>());
            return _overlappingBoosters;
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