using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RogueIslands.Gameplay.View
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

        public event Action DragEnded;

        public bool AllowReorder
        {
            get => _allowReorder;
            set => _allowReorder = value;
        }

        private void Awake()
        {
            TargetPosition = transform.position;
        }

        private void Update()
        {
            if (ShouldAnimateToTarget && Owner != null)
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

        private void OnTransformParentChanged()
        {
            TargetPosition = transform.position;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!AllowReorder)
                return;

            ShouldAnimateToTarget = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!AllowReorder)
                return;

            transform.position = eventData.position;

            if (Owner != null) 
                Owner.Reorder(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!AllowReorder)
                return;

            ShouldAnimateToTarget = true;

            DragEnded?.Invoke();
        }
    }
}