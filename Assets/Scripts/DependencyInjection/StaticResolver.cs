using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RogueIslands.DependencyInjection
{
    public static class StaticResolver
    {
        private static readonly List<IContainer> _containers = new();
        
        public static T Resolve<T>()
        {
            if (_containers.Count == 0)
                throw new Exception("No container has been registered");
            
            return _containers[^1].Resolve<T>();
        }

        public static void AddContainer(IContainer container) 
            => _containers.Add(container);

        public static void RemoveContainer(IContainer container) 
            => _containers.Remove(container);

#if UNITY_EDITOR
        [InitializeOnEnterPlayMode]
        private static void Reset()
        {
            _containers.Clear();
        }
#endif
    }
}