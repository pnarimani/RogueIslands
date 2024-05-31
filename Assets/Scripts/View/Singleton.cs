using UnityEngine;

namespace RogueIslands.View
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            Instance = (T)this;
        }
    }
}