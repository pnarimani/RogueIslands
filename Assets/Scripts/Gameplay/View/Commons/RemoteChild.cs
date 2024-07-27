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
        
        public void SetParent(Transform parent, Vector3 localPosition)
        {
            _camera = Camera.main;
            _parent = parent;
            _localPosition = localPosition;
            _isWorldSpaceUI = _parent.GetComponentInParent<Canvas>().renderMode == RenderMode.WorldSpace;
            SetPosition();
        }

        private void Update()
        {
            if (_parent == null)
            {
                Destroy(gameObject);
                return;
            }
            
            SetPosition();
        }

        private void SetPosition()
        {
            var parentPosition = _isWorldSpaceUI
                ? _camera.WorldToScreenPoint(_parent.position)
                : _parent.position;

            transform.position = parentPosition + _localPosition;
        }
    }
}