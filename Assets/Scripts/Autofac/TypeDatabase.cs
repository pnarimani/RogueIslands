using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using RogueIslands.Diagnostics;

namespace RogueIslands.Autofac
{
    public static class TypeDatabase
    {
        private static List<Type> _allTypes;
        private static List<Type> _projectTypes;
        private static readonly Dictionary<Type, List<Type>> _typesByBaseType = new();
        private static readonly Dictionary<Type, List<Type>> _projectTypesByBaseType = new();

        public static IReadOnlyList<Type> GetAll()
        {
            using var profiler = new ProfilerScope("TypeDatabase.GetAll");
            
            _allTypes ??= AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(IsInstantiatable)
                .ToList();

            return _allTypes;
        }

        public static IReadOnlyList<Type> GetProjectTypes()
        {
            using var profiler = new ProfilerScope("TypeDatabase.GetProjectTypes");
            
            _projectTypes ??= AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.FullName.Contains("RogueIslands"))
                .SelectMany(x => x.GetTypes())
                .Where(IsInstantiatable)
                .ToList();

            return _projectTypes;
        }

        public static IReadOnlyList<Type> GetTypesOf<T>()
        {
            using var profiler = new ProfilerScope("TypeDatabase.GetTypesOf");
            return GetCachedSubTypes<T>(_typesByBaseType, () => GetAll().Where(typeof(T).IsAssignableFrom).ToList());
        }

        public static IReadOnlyList<Type> GetProjectTypesOf<T>()
        {
            using var profiler = new ProfilerScope("TypeDatabase.GetProjectTypesOf");
            
            return GetCachedSubTypes<T>(_projectTypesByBaseType,
                () => GetProjectTypes().Where(typeof(T).IsAssignableFrom).ToList());
        }

        private static bool IsInstantiatable(Type x)
        {
            return !x.IsAbstract &&
                   x.IsClass &&
                   x.GetCustomAttribute<CompilerGeneratedAttribute>() == null &&
                   !x.IsGenericTypeDefinition &&
                   !typeof(Delegate).IsAssignableFrom(x);
        }

        private static List<Type> GetCachedSubTypes<T>(Dictionary<Type, List<Type>> dictionary,
            Func<List<Type>> builder)
        {
            var baseType = typeof(T);
            if (!dictionary.TryGetValue(baseType, out var types))
            {
                types = builder();
                dictionary[baseType] = types;
            }

            return types;
        }
    }
}