using System;
using System.Linq;
using Autofac;
using RogueIslands.Autofac;
using RogueIslands.Gameplay.Boosters.Evaluators;
using RogueIslands.Gameplay.Boosters.Executors;

namespace RogueIslands.Gameplay.Modules
{
    public class BoostersModule : IModule
    {
        public void Load(ContainerBuilder builder)
        {
            foreach (var evalType in TypeDatabase.GetProjectTypesOf<GameConditionEvaluator>())
            {
                builder.RegisterType(evalType)
                    .As<GameConditionEvaluator>();
            }

            foreach (var execType in TypeDatabase.GetProjectTypesOf<GameActionExecutor>())
            {
                builder.RegisterType(execType)
                    .As<GameActionExecutor>();
            }
        }
    }
}