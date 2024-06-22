using System;
using System.Linq;
using Autofac;
using RogueIslands.Rollback;

namespace RogueIslands.Autofac
{
    public class RollbackModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var allTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract)
                .ToList();

            foreach (var handlerType in allTypes.Where(t => typeof(IStateRestoreHandler).IsAssignableFrom(t)))
            {
                builder.RegisterType(handlerType).As<IStateRestoreHandler>();
            }
        }
    }
}