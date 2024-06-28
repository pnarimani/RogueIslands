using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;

namespace RogueIslands.Autofac
{
    public static class ModuleFinder
    {
        public static bool IsGameplayModule(Type type)
            => type.Namespace != null && type.Namespace.Contains("Gameplay");

        public static IEnumerable<Module> GetGameplayModules()
        {
            return GetModuleTypes()
                .Where(IsGameplayModule)
                .Select(x => (Module)Activator.CreateInstance(x));
        }
        
        public static IEnumerable<Module> GetProjectModules()
        {
            return GetModuleTypes()
                .Where(x => !IsGameplayModule(x))
                .Select(x => (Module)Activator.CreateInstance(x));
        }

        private static IEnumerable<Type> GetModuleTypes()
        {
#if UNITY_EDITOR
            return UnityEditor.TypeCache.GetTypesDerivedFrom<Module>();
#endif
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => x.IsSubclassOf(typeof(Module)));
        }
    }
}