using Autofac;
using AutofacUnity;
using RogueIslands.Gameplay;

namespace RogueIslands.DependencyInjection.Autofac
{
    public class MenuLifetimeScope : AutofacScope, IResolver
    {
        public T Resolve<T>()
        {
            if(Container == null)
                Build();
            
            return Container!.Resolve<T>();
        }
    }
}