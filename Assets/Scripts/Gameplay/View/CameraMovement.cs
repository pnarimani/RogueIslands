using System.Linq;
using Unity.Cinemachine;
using UnityEngine;

namespace RogueIslands.Gameplay.View
{
    public class CameraMovement : SingletonMonoBehaviour<CameraMovement>
    {
        [SerializeField] private Transform _target;
        [SerializeField] private CinemachineOrbitalFollow _cinemachineCamera;

        private Bounds _bounds;

        private const float ScrollMultiplier = 1f;
        private float _radiusTarget;
        private float _horizontalAxisValue;

        private void Start()
        {
            _bounds = new Bounds(Vector3.zero, Vector3.zero);

            foreach (var o in GameObject.FindGameObjectsWithTag("Environment"))
            {
                foreach (var r in o.GetComponentsInChildren<MeshRenderer>())
                {
                    _bounds.Encapsulate(r.bounds);
                }
            }

            _bounds.Expand(30);

            InputHandling.Instance.Drag += OnDrag;
            InputHandling.Instance.Scroll += OnScroll;
            InputHandling.Instance.AltDrag += OnAltDrag;

            _radiusTarget = _cinemachineCamera.Radius;
            _horizontalAxisValue = _cinemachineCamera.HorizontalAxis.Value;

            ClampRadiusTarget();
        }

        private void OnAltDrag(Vector2 obj)
        {
            _horizontalAxisValue += obj.x * 0.1f;
            _horizontalAxisValue = (_horizontalAxisValue + 360) % 360;
        }

        private void OnScroll(float obj)
        {
            if (ObjectRegistry.GetBuildingCards().Any(c => c.IsSelected))
                return;
            _radiusTarget += -obj * ScrollMultiplier;
            ClampRadiusTarget();
        }

        private void ClampRadiusTarget()
        {
            var position = _target.transform.position + Vector3.up * 500;
            var max = _bounds.max.y;
            if (Physics.Raycast(position, Vector3.down, out var hit, 1000, LayerMask.GetMask("Ground")))
                max = (hit.point.y / Mathf.Sin(Mathf.Deg2Rad * 55)) + 15;
            _radiusTarget = Mathf.Clamp(_radiusTarget, max, _bounds.max.y + 80);
        }

        private void Update()
        {
            _cinemachineCamera.Radius = Mathf.Lerp(_cinemachineCamera.Radius, _radiusTarget, 20 * Time.deltaTime);
            _cinemachineCamera.HorizontalAxis.Value = Mathf.LerpAngle(_cinemachineCamera.HorizontalAxis.Value,
                _horizontalAxisValue, 10 * Time.deltaTime);

            ClampRadiusTarget();
        }

        private void OnDrag(Vector2 obj)
        {
            var xz = new Vector3(-obj.x, 0, -obj.y);
            xz = Quaternion.Euler(0, _cinemachineCamera.HorizontalAxis.Value, 0) * xz;
            var pos = _target.position + xz * 0.03f;

            pos.x = Mathf.Clamp(pos.x, _bounds.min.x, _bounds.max.x);
            pos.z = Mathf.Clamp(pos.z, _bounds.min.z, _bounds.max.z);

            _target.position = pos;
        }
    }
}