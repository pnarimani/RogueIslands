using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace RogueIslands.View
{
    public class CardListView : MonoBehaviour
    {
        [FormerlySerializedAs("_center")] [SerializeField]
        private bool _fitContent = true;

        [SerializeField] private float _minPadding = 5;
        [SerializeField] private Vector2 _offset;
        [SerializeField] private Vector2 _padding;
        [SerializeField] private Vector2 _minSize, _maxSize;
        [SerializeField] private RectTransform _content;

        private readonly List<CardListItem> _items = new();

        private RectTransform Tx => (RectTransform)transform;

        public RectTransform Content => _content;
        public IReadOnlyList<CardListItem> Items => _items;

        public void Add(CardListItem item)
        {
            if (item.transform.parent != Content)
                item.transform.SetParent(Content, false);

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
            if (_fitContent)
                Tx.sizeDelta = Vector2.Lerp(Tx.sizeDelta, GetContentSize(), 10 * Time.deltaTime);
        }

        private void UpdatePositions()
        {
            _items.RemoveAll(x => x == null);
            if (_items.Count == 0)
                return;

            for (var i = _items.Count - 1; i >= 0; i--)
            {
                var item = _items[i];
                item.TargetPosition = GetPositionForIndex(i);
                item.ExtraAnimationPositionOffset = AnimatePosition(i);
                item.ExtraAnimationRotationOffset = GetRotationForIndex(i);
            }
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
            var contentSize = GetContentSize() - _padding;
            var maxSpacePerItem = contentSize.x / _items.Count;

            var spacePerItem = _fitContent
                ? Mathf.Min(maxSpacePerItem, GetFirstItemRect().width + _minPadding)
                : maxSpacePerItem;

            var startingPosition = _fitContent
                ? worldRect.center + Vector2.left * ((_items.Count - 1) / 2f * spacePerItem)
                : worldRect.min + Vector2.up * worldRect.height / 2;

            return startingPosition + Vector2.right * (index * spacePerItem) + _offset;
        }

        private Vector2 GetContentSize()
        {
            if (_items.Count == 0)
                return _minSize;
            var itemRect = GetFirstItemRect();
            var spacePerItem = itemRect.width + _minPadding;
            var size = new Vector2(_items.Count * spacePerItem, itemRect.height) + _padding;
            size.x = Mathf.Max(size.x, _minSize.x);
            size.y = Mathf.Max(size.y, _minSize.y);
            size.x = Mathf.Min(size.x, _maxSize.x);
            size.y = Mathf.Min(size.y, _maxSize.y);
            return size;
        }

        private Rect GetFirstItemRect()
            => ((RectTransform)_items[0].transform).rect;

        private Vector2 AnimatePosition(int index)
            => new(0, Mathf.Sin(Time.time * 2 + index * 0.5f) * 4);

        private Quaternion GetRotationForIndex(int i)
            => Quaternion.identity * Quaternion.Euler(0, 0, Mathf.Sin(Time.time * 1 + i * 0.5f) * 1);
    }
}