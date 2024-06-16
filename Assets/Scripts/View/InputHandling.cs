using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RogueIslands.View
{
    public class InputHandling : SingletonMonoBehaviour<InputHandling>
    {
        private bool _mouseMoved;
        private Vector3 _previousMousePosition;
        private bool _validMouseEvent;

        public event Action Click;
        public event Action<Vector2> Drag;
    
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _validMouseEvent = !EventSystem.current.IsPointerOverGameObject();
                _previousMousePosition = Input.mousePosition;
            }
        
            if(!_validMouseEvent) return;
        
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
    }
}