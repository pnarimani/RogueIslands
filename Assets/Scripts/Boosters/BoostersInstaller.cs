using System;
using System.Linq;
using VContainer;
using VContainer.Unity;

namespace RogueIslands.Boosters
{
    public class BoostersInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.Register(obj => obj.Resolve<Random>().NextRandom(), Lifetime.Transient);

            var allTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract)
                .ToList();

            foreach (var evalType in allTypes.Where(t =>
                         typeof(ConditionEvaluator).IsAssignableFrom(t) &&
                         !typeof(IEvaluationConditionOverride).IsAssignableFrom(t)))
            {
                builder.Register(evalType, Lifetime.Scoped).As<ConditionEvaluator>();
            }

            foreach (var execType in allTypes.Where(t => typeof(GameActionExecutor).IsAssignableFrom(t)))
            {
                builder.Register(execType, Lifetime.Scoped).As<GameActionExecutor>();
            }
        }
    }
}