using System;
using System.Collections.Generic;
using Autofac;
using UnityEditor;

namespace RogueIslands.Autofac
{
    public static class StaticResolver
    {
        private static readonly List<ILifetimeScope> _containers = new();
        
        public static T Resolve<T>()
        {
            if (_containers.Count == 0)
                throw new Exception("No container has been registered");
            
            return _containers[^1].Resolve<T>();
        }

        public static void AddContainer(ILifetimeScope container) 
            => _containers.Add(container);

        public static void RemoveContainer(ILifetimeScope container) 
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