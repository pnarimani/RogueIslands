using UnityEngine;

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
    }
}