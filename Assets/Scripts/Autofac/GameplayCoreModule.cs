using System;
using Autofac;
using RogueIslands.Rollback;

namespace RogueIslands.Autofac
{
    public class GameplayCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register((c) => c.Resolve<Random>().NextRandom())
                .InstancePerDependency();
            
            builder.RegisterModule<BoostersModule>();
            builder.RegisterModule<RollbackModule>();
            
            builder.Register(c => GameFactory.NewGame(c.Resolve<Random>()))
                            .SingleInstance()
                            .AsSelf();

            builder.RegisterType<PlayController>().SingleInstance();
            builder.RegisterType<EventController>().SingleInstance();
            builder.RegisterType<GameActionController>().SingleInstance();
            builder.RegisterType<GameConditionsController>().SingleInstance();
            builder.RegisterType<BoosterManagement>().SingleInstance();
            builder.RegisterType<ResetController>().SingleInstance();
        }
    }
}