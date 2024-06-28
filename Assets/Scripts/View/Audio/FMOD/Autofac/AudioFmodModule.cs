using System;
using System.Linq;
using Autofac;

namespace RogueIslands.Autofac.Modules
{
    public class AudioFmodModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.FullName.Contains("RogueIslands.View.Audio"))
                .SelectMany(x => x.GetTypes())
                .Where(x => !x.IsAbstract)
                .ToArray();

            builder.RegisterTypes(types).AsImplementedInterfaces().SingleInstance();
        }
    }
}