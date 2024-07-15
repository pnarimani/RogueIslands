using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RogueIslands.Gameplay.View
{
    public class InputHandling : SingletonMonoBehaviour<InputHandling>
    {
        private bool _mouseMoved;
        private Vector3 _previousMousePosition;
        private ClickState _lastClickState;
        private readonly List<RaycastResult> _raycastResults = new();
        private int _uiLayer;

        public event Action Click;
        public event Action<Vector2> Drag;
        public event Action<Vector2> AltDrag;
        public event Action<float> Scroll;

        protected override void Awake()
        {
            base.Awake();

            _uiLayer = LayerMask.NameToLayer("UI");
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _lastClickState = IsValidMouseEvent() ? ClickState.ValidLeft : ClickState.Invalid;
                _previousMousePosition = Input.mousePosition;
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                _lastClickState = IsValidMouseEvent() ? ClickState.ValidRight : ClickState.Invalid;
                _previousMousePosition = Input.mousePosition;
            }
            
            if (Input.mouseScrollDelta.y != 0)
            {
                Scroll?.Invoke(Input.mouseScrollDelta.y);
            }

            if (_lastClickState == ClickState.Invalid)
                return;

            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                var delta = Input.mousePosition - _previousMousePosition;
                if (delta.sqrMagnitude > 10f)
                {
                    _mouseMoved = true;
                }

                if (_mouseMoved)
                {
                    if (_lastClickState == ClickState.ValidLeft)
                        Drag?.Invoke(delta);
                    else if (_lastClickState == ClickState.ValidRight)
                        AltDrag?.Invoke(delta);
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

        private enum ClickState
        {
            Invalid,
            ValidLeft,
            ValidRight
        }
    }
}