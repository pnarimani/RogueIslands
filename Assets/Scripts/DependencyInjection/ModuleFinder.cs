using System;
using System.Collections.Generic;
using System.Linq;

namespace RogueIslands.DependencyInjection
{
    public static class ModuleFinder
    {
        public static bool IsGameplayModule(Type type)
            => type.Namespace != null && type.Namespace.Contains("Gameplay");

        public static IEnumerable<IModule> GetGameplayModules()
        {
            return GetModuleTypes()
                .Where(IsGameplayModule)
                .Select(x => (IModule)Activator.CreateInstance(x));
        }
        
        public static IEnumerable<IModule> GetProjectModules()
        {
            return GetModuleTypes()
                .Where(x => !IsGameplayModule(x))
                .Select(x => (IModule)Activator.CreateInstance(x));
        }

        private static IEnumerable<Type> GetModuleTypes()
        {
#if UNITY_EDITOR
            return UnityEditor.TypeCache.GetTypesDerivedFrom<IModule>();
#endif
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => x.IsSubclassOf(typeof(IModule)));
        }
    }
}