using System;
using System.Linq;
using RogueIslands.DependencyInjection;
using RogueIslands.Gameplay.DeckBuilding;
using RogueIslands.Gameplay.DeckBuilding.ActionHandlers;

namespace RogueIslands.Gameplay.Modules
{
    public class DeckBuildingModule : IModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<DeckBuildingController>()
                .SingleInstance()
                .AsSelf()
                .AsImplementedInterfaces();
            
            foreach (var handlerType in TypeDatabase.GetProjectTypesOf<DeckActionHandler>())
            {
                builder.RegisterType(handlerType).As<DeckActionHandler>();
            }
        }
    }
}