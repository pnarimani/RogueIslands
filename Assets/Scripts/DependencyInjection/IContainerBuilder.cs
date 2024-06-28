using System;
using UnityEngine;

namespace RogueIslands.DependencyInjection
{
    public interface IContainerBuilder
    {
        IRegistration<T> RegisterType<T>();
        IRegistration<T> Register<T>(Func<IContainer, T> factory);
        IRegistration<object> RegisterType(Type type);
        IRegistration<T> RegisterInstance<T>(T instance) where T : class;
        IRegistration<T> RegisterMonoBehaviour<T>(T prefab = null) where T : Component;
    }
}