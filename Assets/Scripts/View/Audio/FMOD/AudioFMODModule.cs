using System.Linq;
using RogueIslands.DependencyInjection;
using Object = UnityEngine.Object;

namespace RogueIslands.View.Audio.FMOD
{
    public class AudioFMODModule : IModule
    {
        public void Load(IContainerBuilder builder)
        {
            var types = TypeDatabase.GetProjectTypes()
                .Where(x => x.Assembly.FullName.Contains("RogueIslands.View.Audio"))
                .Where(x => !typeof(Object).IsAssignableFrom(x));

            foreach (var t in types)
            {
                builder.RegisterType(t).AsImplementedInterfaces().SingleInstance();
            }
        }
    }
}