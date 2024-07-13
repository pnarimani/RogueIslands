using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RogueIslands.Gameplay.View
{
    public abstract class Singleton<T> : IDisposable where T : Singleton<T>
    {
        public static T Instance { get; private set; }

        protected Singleton()
        {
            Instance = (T)this;

#if UNITY_EDITOR
            EditorApplication.playModeStateChanged += Reset;

            void Reset(PlayModeStateChange obj)
            {
                if (obj == PlayModeStateChange.ExitingPlayMode)
                {
                    Instance = null;
                    EditorApplication.playModeStateChanged -= Reset;
                }
            }
#endif
        }

        public virtual void Dispose()
        {
            Instance = null;
        }
    }
}