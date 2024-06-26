using Autofac;
using RogueIslands.Serialization.XML;

namespace RogueIslands.Autofac.Modules
{
    public class XmlSerializationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<XmlProxy>().AsImplementedInterfaces().SingleInstance();
        }
    }
}