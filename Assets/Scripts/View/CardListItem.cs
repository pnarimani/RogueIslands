using UnityEngine;
using UnityEngine.EventSystems;

namespace RogueIslands.View
{
    public class CardListItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private float _speed = 10;
        [SerializeField] private bool _allowReorder;
    
        public bool ShouldAnimateToTarget { get; set; } = false;
        public Vector3 TargetPosition { get; set; }
        public CardListView Owner { get; set; }

        public bool AllowReorder
        {
            get => _allowReorder;
            set => _allowReorder = value;
        }

        private void Update()
        {
            if (ShouldAnimateToTarget)
            {
                transform.position = Vector3.Lerp(transform.position, TargetPosition, Time.deltaTime * _speed);
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
        }
    }
}