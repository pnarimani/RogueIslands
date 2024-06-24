using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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

                SceneManager.sceneLoaded -= ResetScope;
                SceneManager.sceneLoaded += ResetScope;

                if (_scope == null)
                {
                    Debug.LogError("Failed to find resolver");
                    return default;
                }
            }
            
            return _scope.Resolve<T>();
        }

        private static void ResetScope(Scene arg0, LoadSceneMode loadSceneMode)
        {
            _scope = null;
            SceneManager.sceneLoaded -= ResetScope;
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