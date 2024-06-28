using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RogueIslands.Gameplay.View
{
    public class CardHoverAnimations : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Transform _target;
        private bool _entered;

        private void Update()
        {
            if (!_entered)
                return;

            var rect = transform.GetWorldRect();
            var mousePosition = (Vector2)Input.mousePosition;
            if (rect.Contains(mousePosition))
            {
                var tx = Mathf.InverseLerp(rect.min.x, rect.max.x, mousePosition.x);
                var ty = Mathf.InverseLerp(rect.min.y, rect.max.y, mousePosition.y);
                var deltaX = Mathf.Lerp(-20, 20, tx);
                var deltaY = Mathf.Lerp(0, 20, ty);
                _target.localRotation = Quaternion.Euler(deltaY, -deltaX, 0);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _target.DOPunchScale(0.05f * Vector3.one, 0.2f);
            _entered = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _target.localRotation = Quaternion.identity;
            _entered = false;
        }
    }
}