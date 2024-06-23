using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RogueIslands.View
{
    public class CardListItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private float _speed = 10;
        [SerializeField] private bool _allowReorder;
        [SerializeField] private Transform _extraAnimationParent;

        public bool ShouldAnimateToTarget { get; set; } = false;
        public Vector2 TargetPosition { get; set; }
        public Vector2 ExtraAnimationPositionOffset { get; set; }
        public Quaternion ExtraAnimationRotationOffset { get; set; }
        public CardListView Owner { get; set; }

        public event Action CardReordered;

        public bool AllowReorder
        {
            get => _allowReorder;
            set => _allowReorder = value;
        }

        private void Update()
        {
            if (ShouldAnimateToTarget)
            {
                transform.position = Vector2.Lerp(transform.position, TargetPosition, Time.deltaTime * _speed);
            }

            if (_extraAnimationParent != null)
            {
                _extraAnimationParent.localRotation = Quaternion.Slerp(_extraAnimationParent.localRotation,
                    ExtraAnimationRotationOffset, Time.deltaTime * _speed);
                _extraAnimationParent.localPosition = Vector2.Lerp(_extraAnimationParent.localPosition,
                    ExtraAnimationPositionOffset, Time.deltaTime * _speed);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!AllowReorder)
                return;
            if (Owner == null)
                return;

            ShouldAnimateToTarget = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!AllowReorder)
                return;
            if (Owner == null)
                return;

            transform.position = eventData.position;

            Owner.Reorder(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!AllowReorder)
                return;
            if (Owner == null)
                return;

            ShouldAnimateToTarget = true;

            CardReordered?.Invoke();
        }
    }
}