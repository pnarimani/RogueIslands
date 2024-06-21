using UnityEngine;

namespace RogueIslands.View
{
    public class BuildingViewPlacement : Singleton<BuildingViewPlacement>, IGizmosDrawer
    {
        private readonly Camera _camera = Camera.main;
        private readonly int _layerMask = LayerMask.GetMask("Building");
        private readonly int _groundMask = LayerMask.GetMask("Ground");

        private Vector3 _gizmosCenter, _gizmosSize;

        public Vector3 GetPosition(Transform building)
        {
            var currentBuildingPosition = building.position;
            var bounds = building.GetCollisionBounds();

            var ray = _camera!.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit, 100, _groundMask))
                return Vector3.zero;

            var desiredPosition = hit.point;

            foreach (var other in Object.FindObjectsOfType<BuildingView>())
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

        public void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(_gizmosCenter, _gizmosSize);
        }
    }
}