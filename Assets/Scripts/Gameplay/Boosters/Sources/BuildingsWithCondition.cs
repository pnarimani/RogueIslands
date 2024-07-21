using System.Collections.Generic;
using RogueIslands.DependencyInjection;
using RogueIslands.Gameplay.Boosters.Conditions;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.Boosters.Sources
{
    public class BuildingsWithCondition : ISource<Building>
    {
        public ISource<Building> Source { get; set; }
        public IGameConditionWithSource<Building> Condition { get; set; }

        public IEnumerable<Building> Get(GameState state, IBooster booster)
        {
            var controller = StaticResolver.Resolve<GameConditionsController>();
            var instance = new Instance<Building>();
            Condition.Source = instance;
            foreach (var building in Source.Get(state, booster))
            {
                instance.Value = building;
                if (controller.IsConditionMet(booster, Condition))
                    yield return building;
            }
        }
    }
}