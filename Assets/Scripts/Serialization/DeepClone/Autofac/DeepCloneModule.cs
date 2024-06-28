using Autofac;
using RogueIslands.Serialization.DeepClone;

namespace Serialization.DeepClone.Autofac
{
    public class DeepCloneModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(new Cloner()).AsImplementedInterfaces().SingleInstance();
        }
    }
}