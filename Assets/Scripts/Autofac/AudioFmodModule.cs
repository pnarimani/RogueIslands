using Autofac;
using RogueIslands.View.Audio.FMOD;

namespace RogueIslands.Autofac
{
    public class AudioFmodModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BuildingAudio>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ScoreAudio>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<BuildingCardAudio>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<StageAudio>().AsImplementedInterfaces().SingleInstance();
        }
    }
}