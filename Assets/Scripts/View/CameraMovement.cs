using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace RogueIslands.View
{
    public class CameraMovement : SingletonMonoBehaviour<CameraMovement>
    {
        [SerializeField] private Transform _target;
        [SerializeField] private CinemachineOrbitalFollow _cinemachineCamera;

        private const float ScrollMultiplier = 1f;
        private float _radiusTarget;
        private float _horizontalAxisValue;

        private void Start()
        {
            InputHandling.Instance.Drag += OnDrag;
            InputHandling.Instance.Scroll += OnScroll;
            InputHandling.Instance.AltDrag += OnAltDrag;
            
            _radiusTarget = _cinemachineCamera.Radius;
            _horizontalAxisValue = _cinemachineCamera.HorizontalAxis.Value;
        }

        private void OnAltDrag(Vector2 obj)
        {
            _horizontalAxisValue += obj.x * 0.1f;
            _horizontalAxisValue = (_horizontalAxisValue + 360) % 360;
        }

        private void OnScroll(float obj)
        {
            _radiusTarget = Mathf.Clamp(_radiusTarget + -obj * ScrollMultiplier, 10, 50);
        }

        private void Update()
        {
            _cinemachineCamera.Radius = Mathf.Lerp(_cinemachineCamera.Radius, _radiusTarget, 20 * Time.deltaTime);
            _cinemachineCamera.HorizontalAxis.Value = Mathf.LerpAngle(_cinemachineCamera.HorizontalAxis.Value, _horizontalAxisValue, 10 * Time.deltaTime);
        }

        private void OnDrag(Vector2 obj)
        {
            var xz = new Vector3(obj.x, 0, obj.y);
            xz = Quaternion.Euler(0, _cinemachineCamera.HorizontalAxis.Value, 0) * xz;
            _target.position += xz * (Time.deltaTime * -10);
        }
    }
}