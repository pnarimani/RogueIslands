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
            
            var allTypes = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.FullName.Contains("RogueIslands"))
                .SelectMany(x => x.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract)
                .ToList();

            foreach (var handlerType in allTypes.Where(t => typeof(DeckActionHandler).IsAssignableFrom(t)))
            {
                builder.RegisterType(handlerType).As<DeckActionHandler>();
            }
        }
    }
}