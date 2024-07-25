using Autofac;
using Autofac.Builder;
using UnityEngine;

namespace RogueIslands.Autofac
{
    public static class AutofacExtensions
    {
        public static IRegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle> RegisterMonoBehaviour<T>(this ContainerBuilder builder, T prefab = default(T))
            where T : Component
        {
            var registration = builder.Register(_ =>
                prefab != null
                    ? Object.Instantiate(prefab)
                    : new GameObject(typeof(T).Name).AddComponent<T>()
            );

            registration.AsImplementedInterfaces();

            return registration;
        }
    }
}