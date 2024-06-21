using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RogueIslands.View
{
    public class InputHandling : SingletonMonoBehaviour<InputHandling>
    {
        private bool _mouseMoved;
        private Vector3 _previousMousePosition;
        private bool _validMouseEvent;
        private readonly List<RaycastResult> _raycastResults = new();
        private int _uiLayer;

        public event Action Click;
        public event Action<Vector2> Drag;

        protected override void Awake()
        {
            base.Awake();

            _uiLayer = LayerMask.NameToLayer("UI");
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _validMouseEvent = IsValidMouseEvent();
                _previousMousePosition = Input.mousePosition;
            }

            if (!_validMouseEvent) return;

            if (Input.GetMouseButton(0))
            {
                if ((Input.mousePosition - _previousMousePosition).sqrMagnitude > 10f)
                {
                    _mouseMoved = true;
                }

                if (_mouseMoved)
                {
                    Drag?.Invoke(Input.mousePosition - _previousMousePosition);
                    _previousMousePosition = Input.mousePosition;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (!_mouseMoved)
                {
                    Click?.Invoke();
                }

                _mouseMoved = false;
            }
        }

        private bool IsValidMouseEvent()
        {
            _raycastResults.Clear();

            var data = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
            
            EventSystem.current.RaycastAll(data, _raycastResults);

            foreach (var r in _raycastResults)
            {
                if (r.gameObject.layer == _uiLayer)
                    return false;
            }

            return true;
        }
    }
}