using RogueIslands.DependencyInjection;

namespace RogueIslands.View.Audio
{
    internal class AudioModule : IModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<SetInitialGameAudioVolumes>()
                .AutoActivate();
        }
    }
}