using System.Collections.Generic;
using RogueIslands.Gameplay.View.Boosters;
using UnityEngine;

namespace RogueIslands.Gameplay.View
{
    public class DecorationRemover
    {
        private static readonly Collider[] _colliderBuffer = new Collider[1000];
        private static readonly List<GameObject> _overlapping = new();

        
        public static void EnableAllDecorations()
        {
            foreach (var dec in EnvironmentDecoration.All)
            {
                dec.gameObject.SetActive(true);
            }
        }
        
        public static void DisableDecorations(BuildingView building)
        {
            foreach (var dec in EnvironmentDecoration.All)
            {
                dec.gameObject.SetActive(true);
            }

            foreach (var decoration in GetOverlapping(building))
            {
                decoration.SetActive(false);
            }
        }

        public static void RemoveDecorations(BuildingView building)
        {
            for (var i = EnvironmentDecoration.All.Count - 1; i >= 0; i--)
            {
                var dec = EnvironmentDecoration.All[i];
                if (!dec.gameObject.activeSelf)
                    Object.Destroy(dec.gameObject);
            }

            foreach (var decoration in GetOverlapping(building))
            {
                Object.Destroy(decoration);
            }
        }

        public static IReadOnlyList<GameObject> GetOverlapping(BuildingView building)
        {
            var bounds = building.transform.GetBounds(building.GetMeshRenderers());
            var count = Physics.OverlapBoxNonAlloc(bounds.center, bounds.extents, _colliderBuffer,
                building.transform.rotation, LayerMask.GetMask("EnvironmentDecoration"));
            _overlapping.Clear();
            for (var i = 0; i < count; i++)
                _overlapping.Add(_colliderBuffer[i].gameObject);
            return _overlapping;
        }
    }
}