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
            foreach (var handlerType in TypeDatabase.GetProjectTypesOf<IStateRestoreHandler>())
            {
                builder.RegisterType(handlerType).As<IStateRestoreHandler>();
            }
        }
    }
}