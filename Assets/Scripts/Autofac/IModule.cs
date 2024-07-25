using Autofac;

namespace RogueIslands.DependencyInjection
{
    public interface IModule
    {
        void Load(ContainerBuilder builder);
    }
}