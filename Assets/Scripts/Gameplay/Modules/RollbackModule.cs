using System;
using System.Linq;
using RogueIslands.DependencyInjection;
using RogueIslands.Gameplay.Rollback;

namespace RogueIslands.Gameplay.Modules
{
    public class RollbackModule : IModule
    {
        public void Load(IContainerBuilder builder)
        {
            var allTypes = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.FullName.Contains("RogueIslands"))
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