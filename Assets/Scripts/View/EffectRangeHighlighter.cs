using System.Collections.Generic;
using UnityEngine;

namespace RogueIslands.View
{
    public class EffectRangeHighlighter
    {
        private static readonly List<IHighlightable> _highlightables = new();

        public static void Register(IHighlightable highlightable)
            => _highlightables.Add(highlightable);

        public static void Remove(IHighlightable highlightable)
            => _highlightables.Remove(highlightable);

        public static void Highlight(Vector3 center, float range, GameObject exemption = null)
        {
            for (var i = _highlightables.Count - 1; i >= 0; i--)
            {
                var highlightable = _highlightables[i];
                
                if (highlightable == null ||
                    highlightable.transform == null ||
                    highlightable.transform.gameObject == null)
                {
                    _highlightables.RemoveAt(i);
                    continue;
                }
                
                if (highlightable.transform.gameObject == exemption) continue;

                var distance = Vector3.SqrMagnitude(center - highlightable.transform.position);
                highlightable.Highlight(distance <= range * range);
            }
        }
        
        public static void ShowRanges(bool show)
        {
            foreach (var highlightable in _highlightables)
                highlightable.ShowRange(show);
        }

        public static void LowlightAll()
        {
            foreach (var highlightable in _highlightables)
            {
                highlightable.Highlight(false);
                highlightable.ShowRange(false);
            }
        }
    }
}