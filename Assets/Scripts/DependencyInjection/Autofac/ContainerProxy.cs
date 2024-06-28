using Autofac;

namespace RogueIslands.DependencyInjection.Autofac
{
    public class ContainerProxy : IContainer
    {
        private readonly IComponentContext _container;

        public ContainerProxy(IComponentContext container)
        {
            _container = container;
        }

        public T Resolve<T>() => _container.Resolve<T>();
    }
}