using UnityEngine;

namespace RogueIslands.View
{
    public class BuildingViewPlacement : Singleton<BuildingViewPlacement>
    {
        private readonly Camera _camera = Camera.main;
        private readonly int _layerMask = LayerMask.GetMask("Building");

        public Vector3 GetPosition(Vector3 currentBuildingPosition)
        {
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
                    Vector3.one * 0.5f,
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
                    Vector3.one * 0.5f,
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
                    Vector3.one * 0.5f,
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
    }
}