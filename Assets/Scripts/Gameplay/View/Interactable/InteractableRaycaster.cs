using System.Collections.Generic;
using RogueIslands.Gameplay.View;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableRaycaster
{
    private readonly Dictionary<GameObject, InteractableObject> _handles = new();

    private readonly List<RaycastResult> _graphicRaycastResult = new();

    private InteractableObject _hitInteractable;

    private int _raycastFrame;

    private readonly RaycastHit[] _raycastHits = new RaycastHit[10];
    public Vector3 hitPosition;

    public void Register(InteractableObject interactable)
    {
        _handles.Add(interactable.Handle, interactable);
    }

    public void Unregister(InteractableObject interactable)
    {
        _handles.Remove(interactable.Handle);
    }

    public bool IsHit(Vector3 uiPointer, Ray ray, InteractableObject interactable)
    {
        // Check if we've already casted this frame.
        if (_raycastFrame != Time.frameCount)
        {
            _hitInteractable = null;
            _raycastFrame = Time.frameCount;
            var minDistance = float.MaxValue;
            RaycastUI(uiPointer, ref minDistance);
            RaycastPhysics(ray, ref minDistance);
        }

        return _hitInteractable == interactable;
    }

    private void RaycastUI(Vector3 uiPointer, ref float minDistance)
    {
        var eventSystem = EventSystem.current;
        if (eventSystem)
        {
            eventSystem.RaycastAll(new PointerEventData(eventSystem)
            {
                position = uiPointer,
            }, _graphicRaycastResult);

            for (var i = 0; i < _graphicRaycastResult.Count; i++)
            {
                var hit = _graphicRaycastResult[i];
                if (hit.distance < minDistance)
                    if (_handles.TryGetValue(hit.gameObject, out var hitInteractable))
                    {
                        _hitInteractable = hitInteractable;
                        minDistance = hit.distance;

                        hitInteractable.UpdateCanvas();

                        if (hitInteractable.Canvas?.renderMode == RenderMode.ScreenSpaceOverlay)
                            hitPosition = hit.screenPosition;
                        else
                            hitPosition = hit.worldPosition;
                    }
            }
        }
    }

    private void RaycastPhysics(Ray ray, ref float minDistance)
    {
        var hits = Physics.RaycastNonAlloc(ray, _raycastHits, 1000);

        // Find the nearest hit interactable.
        for (var i = 0; i < hits; i++)
        {
            var hit = _raycastHits[i];
            if (hit.distance < minDistance && _handles.TryGetValue(hit.collider.gameObject, out var hitInteractable))
            {
                _hitInteractable = hitInteractable;
                minDistance = hit.distance;
                hitPosition = hit.point;
            }
        }
    }
}