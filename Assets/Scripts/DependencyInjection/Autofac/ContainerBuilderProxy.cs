using System;
using Autofac;
using Autofac.Builder;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RogueIslands.DependencyInjection.Autofac
{
    public class ContainerBuilderProxy : IContainerBuilder
    {
        private readonly ContainerBuilder _builder;

        public ContainerBuilderProxy(ContainerBuilder builder)
        {
            _builder = builder;
        }

        public IRegistration<T> RegisterType<T>()
            => new RegistrationProxy<T, ConcreteReflectionActivatorData>(_builder.RegisterType<T>());

        public IRegistration<T> Register<T>(Func<IContainer, T> factory)
            => new RegistrationProxy<T, SimpleActivatorData>(_builder.Register(c => factory(new ContainerProxy(c))));

        public IRegistration<object> RegisterType(Type type)
            => new RegistrationProxy<object, ConcreteReflectionActivatorData>(_builder.RegisterType(type));

        public IRegistration<T> RegisterInstance<T>(T instance) where T : class
            => new RegistrationProxy<T, SimpleActivatorData>(_builder.RegisterInstance(instance));

        public IRegistration<T> RegisterMonoBehaviour<T>(T prefab = default(T))
            where T : Component
        {
            var registrationBuilder = _builder.Register(_ =>
                prefab != null
                    ? Object.Instantiate(prefab)
                    : new GameObject(typeof(T).Name).AddComponent<T>()
            );

            registrationBuilder.AsImplementedInterfaces();

            return new RegistrationProxy<T, SimpleActivatorData>(registrationBuilder);
        }
    }
}