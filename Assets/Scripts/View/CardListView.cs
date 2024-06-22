using System.Collections.Generic;
using UnityEngine;

namespace RogueIslands.View
{
    public class CardListView : MonoBehaviour
    {
        [SerializeField] private bool _center = true;
        [SerializeField] private float _minPadding = 5;
        [SerializeField] private Vector2 _offset;
        [SerializeField] private Vector2 _padding;
        
        private readonly List<CardListItem> _items = new();

        private RectTransform Tx => (RectTransform)transform;

        public void Add(CardListItem item)
        {
            item.ShouldAnimateToTarget = true;
            item.Owner = this;
            _items.Add(item);
        }

        public void Remove(CardListItem item)
        {
            _items.Remove(item);
        }

        private void Update()
        {
            UpdatePositions();
            if(_center)
                Tx.sizeDelta = Vector2.Lerp(Tx.sizeDelta, GetContentSize(), 10 * Time.deltaTime);
        }

        private void UpdatePositions()
        {
            _items.RemoveAll(x => x == null);
            if (_items.Count == 0)
                return;

            for (var i = _items.Count - 1; i >= 0; i--)
                _items[i].TargetPosition = GetPositionForIndex(i);
        }

        public void Reorder(CardListItem item)
        {
            var index = GetIndexForPosition(item.transform.position);
            if (index < 0 || index >= _items.Count)
                return;

            _items.Remove(item);
            _items.Insert(index, item);
        }

        private int GetIndexForPosition(Vector3 position)
        {
            var min = float.MaxValue;
            var index = -1;

            for (var i = _items.Count - 1; i >= 0; i--)
            {
                var target = GetPositionForIndex(i);
                var distance = Vector3.Distance(position, target);
                if (distance < min)
                {
                    min = distance;
                    index = i;
                }
            }

            return index;
        }

        private Vector2 GetPositionForIndex(int index)
        {
            var worldRect = Tx.GetWorldRect();

            if (_items.Count == 0)
                return worldRect.center;

            var distance = _center
                ? _items[0].transform.GetWorldRect().width + _minPadding
                : worldRect.width / _items.Count;

            var startingPosition = _center
                ? worldRect.center + Vector2.left * ((_items.Count - 1) / 2f * distance)
                : worldRect.min + Vector2.up * worldRect.height / 2;

            return startingPosition + Vector2.right * (index * distance) + _offset;
        }

        private Vector2 GetContentSize()
        {
            if(_items.Count == 0)
                return Vector2.zero;
            var itemRect = _items[0].transform.GetWorldRect();
            return GetPositionForIndex(_items.Count - 1) - GetPositionForIndex(0) +
                   Vector2.right * itemRect.width +
                   Vector2.up * itemRect.height 
                   + _padding;
        }
    }
}