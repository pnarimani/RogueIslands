using System;
using System.Buffers;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace RogueIslands.View
{
    public static class TransformExtensions
    {
        public static void DestroyChildren(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                Object.Destroy(child.gameObject);
            }
        }

        public static Transform FindRecursive(this Transform transform, string name)
        {
            foreach (Transform child in transform)
            {
                if (child.name == name)
                    return child;

                var result = child.FindRecursive(name);
                if (result != null)
                    return result;
            }

            return null;
        }

        public static Rect GetWorldRect(this Transform transform)
        {
            if (transform is not RectTransform rectTransform)
                throw new ArgumentException("transform should be a RectTransform", nameof(transform));

            var corners = ArrayPool<Vector3>.Shared.Rent(4);
            rectTransform.GetWorldCorners(corners);
            var min = corners[0];
            var max = corners[2];
            ArrayPool<Vector3>.Shared.Return(corners);
            return new Rect(min, max - min);
        }

        public static Bounds GetCollisionBounds(this Transform transform)
        {
            using var _ = ListPool<Collider>.Get(out var list);
            transform.GetComponentsInChildren(list);

            var bounds = new Bounds(transform.position, Vector3.zero);
            foreach (var ren in list)
                bounds.Encapsulate(ren.bounds);
            return bounds;
        }
    }
}