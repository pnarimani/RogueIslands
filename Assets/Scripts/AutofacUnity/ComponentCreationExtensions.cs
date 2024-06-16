using Autofac;
using Autofac.Builder;
using UnityEngine;

namespace AutofacUnity
{
    public static class ComponentCreationExtensions
    {
        public static IRegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle> RegisterMonoBehaviour<T>(
            this ContainerBuilder builder)
            where T : MonoBehaviour
        {
            return builder.Register(_ => new GameObject(typeof(T).Name)
                .AddComponent<T>());
        }
    }
}