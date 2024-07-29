using AutofacUnity;

namespace RogueIslands.Autofac
{
    public class StaticallyResolvableLifetimeScope : AutofacScope
    {
        public override void Build()
        {
            base.Build();

            StaticResolver.AddContainer(Container);
        }
    }
}