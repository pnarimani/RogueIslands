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

        private void Start()
        {
            InputHandling.Instance.Drag += OnDrag;
            InputHandling.Instance.Scroll += OnScroll;
            InputHandling.Instance.AltDrag += OnAltDrag;
            
            _radiusTarget = _cinemachineCamera.Radius;
        }

        private void OnAltDrag(Vector2 obj)
        {
            _cinemachineCamera.HorizontalAxis.Value += obj.x * Time.deltaTime * 30;
        }

        private void OnScroll(float obj)
        {
            _radiusTarget = Mathf.Clamp(_radiusTarget + -obj * ScrollMultiplier, 10, 50);
        }

        private void Update()
        {
            _cinemachineCamera.Radius = Mathf.Lerp(_cinemachineCamera.Radius, _radiusTarget, 20 * Time.deltaTime);
        }

        private void OnDrag(Vector2 obj)
        {
            var xz = new Vector3(obj.x, 0, obj.y);
            xz = Quaternion.Euler(0, _cinemachineCamera.HorizontalAxis.Value, 0) * xz;
            _target.position += xz * (Time.deltaTime * -10);
        }
    }
}