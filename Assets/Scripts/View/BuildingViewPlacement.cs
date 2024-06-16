using UnityEngine;

namespace RogueIslands.View
{
    public class BuildingViewPlacement : Singleton<BuildingViewPlacement>
    {
        private readonly Camera _camera = Camera.main;
        private readonly int _layerMask = LayerMask.GetMask("Building");

        public Vector3 GetPosition(Transform building)
        {
            var currentBuildingPosition = building.position;
            var bounds = GetBounds(building);

            var ray = _camera!.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit, 100, LayerMask.GetMask("Ground")))
                return Vector3.zero;

            var dir = (hit.point - currentBuildingPosition).normalized;
            var xDir = new Vector3(dir.x, 0, 0);
            var zDir = new Vector3(0, 0, dir.z);

            if (
                Vector3.Distance(hit.point, currentBuildingPosition) > 2f ||
                !Physics.BoxCast(
                    currentBuildingPosition,
                    bounds.extents,
                    (hit.point - currentBuildingPosition).normalized,
                    Quaternion.identity,
                    Vector3.Distance(hit.point, currentBuildingPosition),
                    _layerMask
                )
            )
            {
                return hit.point;
            }

            if (
                !Physics.BoxCast(
                    currentBuildingPosition,
                    bounds.extents,
                    xDir,
                    Quaternion.identity,
                    Vector3.Distance(hit.point, currentBuildingPosition),
                    _layerMask
                )
            )
            {
                var pos = currentBuildingPosition;
                pos.x = hit.point.x;
                return pos;
            }

            if (
                !Physics.BoxCast(
                    currentBuildingPosition,
                    bounds.extents,
                    zDir,
                    Quaternion.identity,
                    Vector3.Distance(hit.point, currentBuildingPosition),
                    _layerMask
                )
            )
            {
                var pos = currentBuildingPosition;
                pos.z = hit.point.z;
                return pos;
            }

            return hit.point;
        }

        private static Bounds GetBounds(Transform building)
        {
            var bounds = new Bounds(building.position, Vector3.zero);
            foreach (var renderer in building.GetComponentsInChildren<Renderer>())
                if (renderer.GetComponent<Collider>())
                    bounds.Encapsulate(renderer.bounds);
            return bounds;
        }
    }
}