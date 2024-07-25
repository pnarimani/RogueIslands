using Autofac;
using RogueIslands.DependencyInjection;

namespace RogueIslands.View.Audio
{
    internal class AudioModule : IModule
    {
        public void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SetInitialGameAudioVolumes>()
                .AutoActivate();
        }
    }
}