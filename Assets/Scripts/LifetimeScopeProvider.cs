using UnityEngine;
using VContainer.Unity;

namespace RogueIslands
{
    public static class LifetimeScopeProvider
    {
        private static LifetimeScope _scope;
        
        public static LifetimeScope Get()
        {
            if (_scope == null)
            {
                _scope = Object.FindAnyObjectByType<GameplayLifetimeScope>();
            }
            
            return _scope;
        }   
    }
}