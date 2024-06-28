using UnityEngine;

namespace RogueIslands.Gameplay.View
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            Instance = (T)this;
        }
    }
}