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
            return TypeDatabase.GetProjectTypesOf<IModule>()
                .Where(IsGameplayModule)
                .Select(x => (IModule)Activator.CreateInstance(x));
        }
        
        public static IEnumerable<IModule> GetProjectModules()
        {
            return TypeDatabase.GetProjectTypesOf<IModule>()
                .Where(x => !IsGameplayModule(x))
                .Select(x => (IModule)Activator.CreateInstance(x));
        }
    }
}