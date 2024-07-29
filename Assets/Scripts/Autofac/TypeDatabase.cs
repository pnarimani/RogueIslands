using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

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
            _allTypes ??= AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(IsInstantiatable)
                .ToList();

            return _allTypes;
        }

        public static IReadOnlyList<Type> GetProjectTypes()
        {
            _projectTypes ??= GetAll()
                .Where(x => x.Assembly.FullName.Contains("RogueIslands"))
                .ToList();

            return _projectTypes;
        }

        public static IReadOnlyList<Type> GetTypesOf<T>() 
            => GetCachedSubTypes<T>(_typesByBaseType, () => GetAll().Where(typeof(T).IsAssignableFrom).ToList());

        public static IReadOnlyList<Type> GetProjectTypesOf<T>() 
            => GetCachedSubTypes<T>(_projectTypesByBaseType, () => GetProjectTypes().Where(typeof(T).IsAssignableFrom).ToList());

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