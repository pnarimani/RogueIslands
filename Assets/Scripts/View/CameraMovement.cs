using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace RogueIslands.View
{
    public class CameraMovement : SingletonMonoBehaviour<CameraMovement>
    {
        [SerializeField] private Transform _target;

        [FormerlySerializedAs("_follow")] [SerializeField]
        private CinemachineFollow _cinemachineCamera;

        [SerializeField] private float _minTargetHeight = -2.5f, _maxTargetHeight = 30;

        private float _scrollMultiplier = 0.6f;

        private bool _mouseMoved;

        private void Start()
        {
            InputHandling.Instance.Drag += OnDrag;
            InputHandling.Instance.Scroll += OnScroll;
            InputHandling.Instance.AltDrag += OnAltDrag;
        }

        private void OnAltDrag(Vector2 obj)
        {
            _target.Rotate(Vector3.up, obj.x * Time.deltaTime * 50, Space.World);
        }

        private void OnScroll(float obj)
        {
            var pos = _target.position + _cinemachineCamera.transform.forward * (obj * _scrollMultiplier);
            pos.y = Mathf.Clamp(pos.y, _minTargetHeight, _maxTargetHeight);
            _target.position = pos;
        }

        private void OnDrag(Vector2 obj)
        {
            var xz = new Vector3(obj.x, 0, obj.y);
            xz = Quaternion.Euler(0, _target.rotation.eulerAngles.y, 0) * xz;
            _target.position += xz * (Time.deltaTime * -10);
        }
    }
}