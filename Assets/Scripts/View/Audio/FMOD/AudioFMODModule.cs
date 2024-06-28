using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using RogueIslands.DependencyInjection;
using Object = UnityEngine.Object;

namespace RogueIslands.View.Audio.FMOD
{
    public class AudioFMODModule : IModule
    {
        public void Load(IContainerBuilder builder)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.FullName.Contains("RogueIslands.View.Audio"))
                .SelectMany(x => x.GetTypes())
                .Where(x => !x.IsAbstract && x.IsClass && x.GetCustomAttribute<CompilerGeneratedAttribute>() == null)
                .Where(x => !typeof(Object).IsAssignableFrom(x));

            foreach (var t in types)
            {
                builder.RegisterType(t).AsImplementedInterfaces().SingleInstance();
            }
        }
    }
}