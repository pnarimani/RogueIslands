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
            builder.RegisterInstance(new Random());

            var allTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).ToList();
            
            foreach (var evalType in allTypes.Where(t => typeof(ConditionEvaluator).IsAssignableFrom(t)))
                builder.Register(evalType, Lifetime.Scoped).As<ConditionEvaluator>();
            
            foreach (var execType in allTypes.Where(t => typeof(GameActionExecutor).IsAssignableFrom(t)))
                builder.Register(execType, Lifetime.Scoped).As<GameActionExecutor>();
        }
    }
}