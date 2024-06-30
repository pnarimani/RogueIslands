using UnityEngine;

namespace RogueIslands.Gameplay.View
{
    public class BuildingViewPlacement : Singleton<BuildingViewPlacement>, IGizmosDrawer
    {
        private readonly Camera _camera = Camera.main;
        private readonly int _buildingMask = LayerMask.GetMask("Building");
        private readonly int _groundMask = LayerMask.GetMask("Ground");

        private Vector3 _gizmosCenter, _gizmosSize;
        private Vector3 _bottomLeft;
        private Vector3 _topLeft;
        private Vector3 _topRight;
        private Vector3 _bottomRight;

        public Vector3 GetPosition(Transform building)
        {
            var currentBuildingPosition = building.position;
            var bounds = building.GetCollisionBounds();

            var ray = _camera!.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit, 100, _groundMask))
                return Vector3.zero;

            var desiredPosition = hit.point;

            foreach (var other in ObjectRegistry.GetBuildings())
            {
                if (other.transform == building)
                    continue;

                var otherBounds = other.transform.GetCollisionBounds();
                bounds.center = desiredPosition;
                if (!bounds.Intersects(otherBounds))
                    continue;

                bounds.center = new Vector3(currentBuildingPosition.x, currentBuildingPosition.y, desiredPosition.z);
                if (bounds.Intersects(otherBounds))
                    desiredPosition.z = currentBuildingPosition.z;

                bounds.center = new Vector3(desiredPosition.x, currentBuildingPosition.y, currentBuildingPosition.z);
                if (bounds.Intersects(otherBounds))
                    desiredPosition.x = currentBuildingPosition.x;
            }

            return desiredPosition;
        }

        public bool IsValidPlacement(Transform building)
        {
            var bounds = building.GetCollisionBounds();
            
            var min = bounds.min;
            var max = bounds.max;

            _bottomLeft = new Vector3(min.x, bounds.center.y, min.z);
            _topLeft = new Vector3(min.x, bounds.center.y, max.z);
            _topRight = new Vector3(max.x, bounds.center.y, max.z);
            _bottomRight = new Vector3(max.x, bounds.center.y, min.z);

            var rays = new Ray[]
            {
                new(_bottomLeft, Vector3.down),
                new(_topLeft, Vector3.down),
                new(_topRight, Vector3.down),
                new(_bottomRight, Vector3.down),
            };

            float? previousHitDistance = null;
            foreach (var ray in rays)
            {
                if (!Physics.Raycast(ray, out var hit, 20, _groundMask | _buildingMask))
                    return false;

                if (previousHitDistance.HasValue && Mathf.Abs(hit.distance - previousHitDistance.Value) > 1f)
                    return false;
                
                previousHitDistance = hit.distance;
            }

            return true;
        }

        public void OnDrawGizmos()
        {
            Gizmos.DrawRay(_bottomLeft, Vector3.down);
            Gizmos.DrawRay(_topLeft, Vector3.down);
            Gizmos.DrawRay(_topRight, Vector3.down);
            Gizmos.DrawRay(_bottomRight, Vector3.down);
        }
    }
}