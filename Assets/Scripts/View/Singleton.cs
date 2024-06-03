using UnityEngine;

namespace RogueIslands.View
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameObject(typeof(T).Name).AddComponent<T>();
                
                return _instance;
            }
            private set => _instance = value;
        }

        protected virtual void Awake()
        {
            Instance = (T)this;
        }
    }
}