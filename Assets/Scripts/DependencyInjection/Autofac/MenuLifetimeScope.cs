using Autofac;
using AutofacUnity;
using RogueIslands.Gameplay;

namespace RogueIslands.DependencyInjection.Autofac
{
    public class MenuLifetimeScope : AutofacScope, IResolver
    {
        public T Resolve<T>() => Container.Resolve<T>();
    }
}