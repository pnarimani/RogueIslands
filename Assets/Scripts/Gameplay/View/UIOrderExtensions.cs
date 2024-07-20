using UnityEngine;
using UnityEngine.UI;

namespace RogueIslands.Gameplay.View
{
    public static class UIOrderExtensions
    {
        public static void BringToFront<T1>(this T1 obj) where T1 : Component
            => BringToFront(obj.gameObject);

        public static void BringToFront<T1, T2>(this T1 obj, T2 reference) where T1 : Component where T2 : Component
            => BringToFront(obj.gameObject, reference.gameObject);

        public static void BringToFront(this GameObject obj)
            => BringToFront(obj, obj);

        public static void BringToFront(this GameObject obj, GameObject reference)
        {
            var source = reference.GetComponentInParent<Canvas>();
            var canvas = obj.AddComponent<Canvas>();
            if (source.gameObject.GetComponent<GraphicRaycaster>())
                obj.AddComponent<GraphicRaycaster>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = source.sortingOrder + 1;
            canvas.vertexColorAlwaysGammaSpace = source.vertexColorAlwaysGammaSpace;
            canvas.additionalShaderChannels = source.additionalShaderChannels;
        }

        public static void ResetOrder(this GameObject obj)
        {
            var raycaster = obj.GetComponent<GraphicRaycaster>();
            if (raycaster)
                Object.Destroy(raycaster);
            Object.Destroy(obj.GetComponent<Canvas>());
        }

        public static void ResetOrder<T>(this T obj) where T : Component => ResetOrder(obj.gameObject);
    }
}