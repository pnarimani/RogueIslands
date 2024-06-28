using System;
using System.Linq;
using Autofac;
using RogueIslands.Gameplay.Boosters.Evaluators;
using RogueIslands.Gameplay.Boosters.Executors;

namespace RogueIslands.Gameplay.Autofac
{
    public class BoostersModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var allTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract)
                .ToList();

            foreach (var evalType in allTypes.Where(t => typeof(GameConditionEvaluator).IsAssignableFrom(t)))
            {
                builder.RegisterType(evalType)
                    .As<GameConditionEvaluator>();
            }

            foreach (var execType in allTypes.Where(t => typeof(GameActionExecutor).IsAssignableFrom(t)))
            {
                builder.RegisterType(execType)
                    .As<GameActionExecutor>();
            }
        }
    }
}