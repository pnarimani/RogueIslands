using AutofacUnity;

namespace RogueIslands.DependencyInjection.Autofac
{
    public class StaticallyResolvableLifetimeScope : AutofacScope
    {
        public override void Build()
        {
            base.Build();

            StaticResolver.AddContainer(new ContainerProxy(Container));
        }
    }
}