using RogueIslands.Diagnostics;
using UnityEngine;

namespace RogueIslands.Gameplay.View
{
    public class BuildingViewPlacement : Singleton<BuildingViewPlacement>
    {
        private readonly Camera _camera = Camera.main;
        private readonly int _buildingMask = LayerMask.GetMask("Building");
        private readonly int _groundMask = LayerMask.GetMask("Ground");
        private readonly Collider[] _colliderBuffer = new Collider[1000];

        public Vector3 GetPosition(BuildingView building)
        {
            using var profiler = ProfilerScope.Begin();

            var allBounds = building.GetAllBounds();

            var ray = _camera!.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit, 100, _groundMask))
            {
                new Plane(Vector3.up, Vector3.zero).Raycast(ray, out var distance);
                return ray.GetPoint(distance);
            }

            var desiredPosition = SlideDesiredPosition(building.transform, allBounds, hit.point);

            if (Vector3.Distance(desiredPosition, hit.point) > 10f)
            {
                desiredPosition = hit.point;
            }

            return desiredPosition;
        }

        private Vector3 SlideDesiredPosition(Transform building, CompositeBounds bounds, Vector3 desiredPosition)
        {
            using var profiler = ProfilerScope.Begin();

            var currentPos = building.position;

            bounds.MoveCenter(currentPos);
            if (IntersectsAnyBuilding(building, bounds))
            {
                Debug.Log("already intersecting");
                return desiredPosition;
            }

            for (var i = 0; i < 5; i++)
            {
                bounds.MoveCenter(desiredPosition);
                if (!IntersectsAnyBuilding(building, bounds))
                {
                    Debug.Log("No intersection");
                    return desiredPosition;
                }

                var desiredDirection = desiredPosition - currentPos;
                var normalizedDirection = desiredDirection.normalized;
                foreach (var subBounds in bounds.Bounds)
                {
                    var centerOffset = subBounds.center - desiredPosition;
                    var center = currentPos + centerOffset;
                    
                    if (Physics.BoxCast(
                            center,
                            subBounds.extents,
                            normalizedDirection,
                            out var hit,
                            building.rotation,
                            100,
                            _buildingMask
                        ))
                    {
                        var leftOver = (desiredDirection.magnitude - hit.distance) * normalizedDirection;
                        var projected = Vector3.ProjectOnPlane(leftOver, hit.normal);
                        desiredPosition = currentPos + projected;
                        break;
                    }
                }
            }

            return desiredPosition;
        }

        private bool IntersectsAnyBuilding(Transform building, CompositeBounds bounds)
        {
            using var profiler = ProfilerScope.Begin();

            foreach (var subBounds in bounds.Bounds)
            {
                var count = Physics.OverlapBoxNonAlloc(
                    subBounds.center,
                    subBounds.extents,
                    _colliderBuffer,
                    building.rotation,
                    _buildingMask
                );
                for (var i = 0; i < count; i++)
                {
                    if (_colliderBuffer[i].transform.root != building)
                        return true;
                }
            }

            return false;
        }

        private bool IntersectsGround(Transform building, CompositeBounds bounds)
        {
            using var profiler = ProfilerScope.Begin();

            foreach (var b in bounds.Bounds)
            {
                var extents = b.extents;
                extents.y = 0.1f;

                var count = Physics.OverlapBoxNonAlloc(
                    b.center,
                    extents,
                    _colliderBuffer,
                    building.rotation,
                    _groundMask
                );

                for (var i = 0; i < count; i++)
                {
                    if (_colliderBuffer[i].transform.root != building)
                        return true;
                }
            }

            return false;
        }

        public bool IsValidPlacement(BuildingView building)
        {
            using var profiler = ProfilerScope.Begin();

            var bounds = building.GetAllBounds();
            var isIntersectingWithAnyBuildings = IntersectsAnyBuilding(building.transform, bounds);
            var isOnFlatGround = IsOnFlatGround(building.transform, bounds);
            var isIntersectingGround = IntersectsGround(building.transform, bounds);
            //
            // Debug.Log("isIntersectingWithAnyBuildings = " + isIntersectingWithAnyBuildings);
            // Debug.Log("isOnFlatGround = " + isOnFlatGround);

            return !isIntersectingGround && !isIntersectingWithAnyBuildings && isOnFlatGround;
        }

        private bool IsOnFlatGround(Transform building, CompositeBounds allBounds)
        {
            using var profiler = ProfilerScope.Begin();

            foreach (var bounds in allBounds.Bounds)
            {
                var min = -bounds.extents;
                var max = bounds.extents;

                var _bottomLeft = new Vector3(min.x, 0.1f, min.z);
                var _topLeft = new Vector3(min.x, 0.1f, max.z);
                var _topRight = new Vector3(max.x, 0.1f, max.z);
                var _bottomRight = new Vector3(max.x, 0.1f, min.z);

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
                    if (!Physics.Raycast(ray, out var hit, 1.5f, _groundMask | _buildingMask))
                        return false;

                    if (previousHitDistance.HasValue && Mathf.Abs(hit.distance - previousHitDistance.Value) > 1f)
                        return false;

                    previousHitDistance = hit.distance;
                }
            }

            return true;
        }
    }
}