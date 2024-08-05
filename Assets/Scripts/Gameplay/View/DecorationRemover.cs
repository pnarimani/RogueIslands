using UnityEngine;

namespace RogueIslands.Gameplay.View
{
    public class DecorationRemover
    {
        private static readonly Collider[] _colliderBuffer = new Collider[1000];

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

            using var overlapping = GetOverlapping(building);
            
            foreach (var decoration in overlapping)
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

            using var overlapping = GetOverlapping(building);
            
            foreach (var decoration in overlapping)
            {
                Object.Destroy(decoration);
            }
        }

        private static PooledList<GameObject> GetOverlapping(BuildingView building)
        {
            var overlapping = PooledList<GameObject>.CreatePooled();
            var bounds = building.GetAllBounds();
            foreach (var bound in bounds.Bounds)
            {
                var count = Physics.OverlapBoxNonAlloc(bound.center, bound.extents, _colliderBuffer,
                    building.transform.rotation, LayerMask.GetMask("EnvironmentDecoration"));
                for (var i = 0; i < count; i++)
                    overlapping.Add(_colliderBuffer[i].gameObject);
            }

            return overlapping;
        }
    }
}