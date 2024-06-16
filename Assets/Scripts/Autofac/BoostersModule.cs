using System;
using System.Linq;
using Autofac;
using RogueIslands.Boosters;

namespace RogueIslands.Autofac
{
    public class BoostersModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var allTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract)
                .ToList();

            foreach (var evalType in allTypes.Where(t =>
                         typeof(ConditionEvaluator).IsAssignableFrom(t) &&
                         !typeof(IEvaluationConditionOverride).IsAssignableFrom(t)))
            {
                builder.RegisterType(evalType).As<ConditionEvaluator>();
            }

            foreach (var execType in allTypes.Where(t => typeof(GameActionExecutor).IsAssignableFrom(t)))
            {
                builder.RegisterType(execType).As<GameActionExecutor>();
            }
        }
    }
}