using RogueIslands.Diagnostics;
using UnityEngine;

namespace RogueIslands.Gameplay.View
{
    public class BuildingViewPlacement : Singleton<BuildingViewPlacement>, IGizmosDrawer
    {
        private readonly Camera _camera = Camera.main;
        private readonly int _buildingMask = LayerMask.GetMask("Building");
        private readonly int _groundMask = LayerMask.GetMask("Ground");
        private readonly Collider[] _colliderBuffer = new Collider[1000];

        private Vector3 _gizmosCenter, _gizmosSize;
        private Vector3 _bottomLeft;
        private Vector3 _topLeft;
        private Vector3 _topRight;
        private Vector3 _bottomRight;

        public Vector3 GetPosition(BuildingView building)
        {
            using var profiler = ProfilerBlock.Begin();
            
            var bounds = building.transform.GetBounds(building.GetMeshRenderers());

            _gizmosCenter = bounds.center;
            _gizmosSize = bounds.size;

            var ray = _camera!.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit, 100, _groundMask))
            {
                new Plane(Vector3.up, Vector3.zero).Raycast(ray, out var distance);
                return ray.GetPoint(distance);
            }

            var desiredPosition = SlideDesiredPosition(building.transform, bounds, hit.point);

            if (Vector3.Distance(desiredPosition, hit.point) > 10f)
            {
                desiredPosition = hit.point;
            }

            return desiredPosition;
        }

        private Vector3 SlideDesiredPosition(Transform building, Bounds bounds, Vector3 desiredPosition)
        {
            using var profiler = ProfilerBlock.Begin();
            
            var currentPos = building.position;

            bounds.center = currentPos;
            if (IntersectsAnyBuilding(building, bounds))
                return desiredPosition;

            for (var i = 0; i < 5; i++)
            {
                bounds.center = desiredPosition;
                if (!IntersectsAnyBuilding(building, bounds)) 
                    return desiredPosition;

                var desiredDirection = desiredPosition - currentPos;
                var normalizedDirection = desiredDirection.normalized;
                Physics.BoxCast(currentPos, bounds.extents, normalizedDirection, out var hit, building.rotation, 100,
                    _buildingMask);
                var leftOver = (desiredDirection.magnitude - hit.distance) * normalizedDirection;
                var projected = Vector3.ProjectOnPlane(leftOver, hit.normal);
                desiredPosition = currentPos + projected;
            }
            
            return desiredPosition;
        }

        private bool IntersectsAnyBuilding(Transform building, Bounds bounds)
        {
            using var profiler = ProfilerBlock.Begin();
            
            var count = Physics.OverlapBoxNonAlloc(bounds.center, bounds.extents, _colliderBuffer, building.rotation,
                _buildingMask);
            for (var i = 0; i < count; i++)
            {
                if (_colliderBuffer[i].transform.root != building)
                    return true;
            }

            return false;
        }

        public bool IsValidPlacement(BuildingView building)
        {
            using var profiler = ProfilerBlock.Begin();
            
            var bounds = building.transform.GetBounds(building.GetMeshRenderers());
            var isIntersectingWithAnyBuildings = IntersectsAnyBuilding(building.transform, bounds);
            var isOnFlatGround = IsOnFlatGround(building.transform, bounds);
            //
            // Debug.Log("isIntersectingWithAnyBuildings = " + isIntersectingWithAnyBuildings);
            // Debug.Log("isOnFlatGround = " + isOnFlatGround);

            return !isIntersectingWithAnyBuildings && isOnFlatGround;
        }

        private bool IsOnFlatGround(Transform building, Bounds bounds)
        {
            using var profiler = ProfilerBlock.Begin();
            
            var min = -bounds.extents;
            var max = bounds.extents;

            _bottomLeft = new Vector3(min.x, 0.1f, min.z);
            _topLeft = new Vector3(min.x, 0.1f, max.z);
            _topRight = new Vector3(max.x, 0.1f, max.z);
            _bottomRight = new Vector3(max.x, 0.1f, min.z);

            _bottomLeft = building.TransformPoint(_bottomLeft);
            _topLeft = building.TransformPoint(_topLeft);
            _topRight = building.TransformPoint(_topRight);
            _bottomRight = building.TransformPoint(_bottomRight);

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
                if (!Physics.Raycast(ray, out var hit, 0.2f, _groundMask | _buildingMask))
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