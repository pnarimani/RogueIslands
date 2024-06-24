using System;
using System.Collections.Generic;
using Autofac;
using RogueIslands.Boosters;
using RogueIslands.Boosters.Executors;
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
            builder.RegisterType<GameActionController>()
                .OnActivated(c => c.Instance.SetExecutors(c.Context.Resolve<IReadOnlyList<GameActionExecutor>>()))
                .SingleInstance();
            builder.RegisterType<GameConditionsController>()
                .OnActivated(c => c.Instance.SetEvaluators(c.Context.Resolve<IReadOnlyList<GameConditionEvaluator>>()))
                .SingleInstance();
            builder.RegisterType<BoosterManagement>().SingleInstance();
            builder.RegisterType<ResetController>().SingleInstance();
            builder.RegisterType<WorldBoosterGeneration>().SingleInstance();
        }
    }
}