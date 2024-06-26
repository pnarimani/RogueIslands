using System;
using System.Collections.Generic;
using Autofac;
using RogueIslands.Boosters;
using RogueIslands.Boosters.Executors;
using RogueIslands.Rollback;
using RogueIslands.Serialization.DeepClone;

namespace RogueIslands.Autofac.Modules
{
    public class GameplayCoreModule : Module
    {
        private readonly Seed _seed;

        public GameplayCoreModule(Seed seed) => _seed = seed;

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(new Cloner()).AsImplementedInterfaces().SingleInstance();
            
            builder.RegisterInstance(_seed);
            
            builder.Register(c => new Random(_seed.Value.GetHashCode()))
                .SingleInstance();
            
            builder.Register((c) => c.Resolve<Random>().NextRandom())
                .InstancePerDependency();
            
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