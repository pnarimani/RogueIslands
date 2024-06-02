using UnityEngine;

namespace RogueIslands.View
{
    public class CameraMovement : Singleton<CameraMovement>
    {
        private bool _mouseMoved;

        private void Start()
        {
            InputHandling.Instance.Drag += OnDrag;
        }

        private void OnDrag(Vector2 obj)
        {
            var xz = new Vector3(obj.x, 0, obj.y);
            transform.position += xz * (Time.deltaTime * -10);
        }
    }
}