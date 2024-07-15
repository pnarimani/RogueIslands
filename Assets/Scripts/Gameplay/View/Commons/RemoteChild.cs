using System;
using UnityEngine;

namespace RogueIslands.Gameplay.View.Commons
{
    public class RemoteChild : MonoBehaviour
    {
        private Camera _camera;
        private bool _isWorldSpaceUI;
        private Transform _parent;
        private Vector3 _localPosition;

        private void Awake()
        {
            _camera = Camera.main;
        }

        public void SetParent(Transform parent, Vector3 localPosition)
        {
            _parent = parent;
            _localPosition = localPosition;
            _isWorldSpaceUI = _parent.GetComponentInParent<Canvas>().renderMode == RenderMode.WorldSpace;
        }

        private void Update()
        {
            if (_parent == null)
            {
                Destroy(gameObject);
                return;
            }

            var parentPosition = _isWorldSpaceUI
                ? _camera.WorldToScreenPoint(_parent.position)
                : _parent.position;
            
            transform.position = parentPosition + _localPosition;
        }
    }
}