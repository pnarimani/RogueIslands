using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

#endif

namespace RogueIslands
{
    public static class StaticResolver
    {
        private static IResolver _scope;
        
        public static T Resolve<T>()
        {
            if (_scope == null)
            {
                _scope = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                    .Where(c => c is IResolver)
                    .Cast<IResolver>()
                    .FirstOrDefault();

                if (_scope == null)
                {
                    Debug.LogError("Failed to find resolver");
                    return default;
                }
            }
            
            return _scope.Resolve<T>();
        }

#if UNITY_EDITOR
        [InitializeOnEnterPlayMode]
        private static void Reset()
        {
            _scope = null;
        }
#endif
    }
}