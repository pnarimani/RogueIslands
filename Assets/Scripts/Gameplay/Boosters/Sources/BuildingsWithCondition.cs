using System.Collections.Generic;
using RogueIslands.Autofac;
using RogueIslands.Gameplay.Boosters.Conditions;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.Boosters.Sources
{
    public class BuildingsWithCondition : ISource<Building>
    {
        public ISource<Building> Source { get; set; }
        public IGameConditionWithSource<Building> Condition { get; set; }

        public IEnumerable<Building> Get(IBooster booster)
        {
            var instance = new Instance<Building>();
            Condition.Source = instance;
            foreach (var building in Source.Get(booster))
            {
                instance.Value = building;
                if (Condition.Evaluate(booster))
                    yield return building;
            }
        }
    }
}