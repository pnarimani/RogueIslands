using Autofac;

namespace RogueIslands.Autofac
{
    public interface IModule
    {
        void Load(ContainerBuilder builder);
    }
}