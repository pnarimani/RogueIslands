using Autofac;
using RogueIslands.DependencyInjection;

namespace RogueIslands.Serialization.DeepClone
{
    public class DeepCloneModule : IModule
    {
        public void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(new Cloner()).AsImplementedInterfaces().SingleInstance();
        }
    }
}