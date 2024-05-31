using System.Collections.Generic;
using UnityEngine;

namespace RogueIslands.View
{
    public class CardListView : MonoBehaviour
    {
        private readonly List<CardListItem> _items = new();
        private readonly Vector3[] _corners = new Vector3[4];

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
        }

        private void UpdatePositions()
        {
            Tx.GetWorldCorners(_corners);

            var leftCenter = (_corners[0] + _corners[1]) / 2;
            var size = _corners[2] - _corners[0];
            var distance = size / _items.Count;

            for (var i = _items.Count - 1; i >= 0; i--)
            {
                var item = _items[i];
                if (item == null)
                {
                    _items.RemoveAt(i);
                    continue;
                }

                item.TargetPosition = leftCenter + Vector3.right * (distance.x / 2) + Vector3.right * (distance.x * i);
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
            Tx.GetWorldCorners(_corners);

            var leftCenter = (_corners[0] + _corners[1]) / 2;
            var size = _corners[2] - _corners[0];
            var cardDistance = size / _items.Count;

            var min = float.MaxValue;
            var index = -1;

            for (var i = _items.Count - 1; i >= 0; i--)
            {
                var target = leftCenter + Vector3.right * (cardDistance.x / 2) + Vector3.right * (cardDistance.x * i);
                var distance = Vector3.Distance(position, target);
                if (distance < min)
                {
                    min = distance;
                    index = i;
                }
            }

            return index;
        }
    }
}