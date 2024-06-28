using System;
using System.Linq;
using Autofac;
using AutofacUnity;
using IngameDebugConsole;
using UnityEngine;

namespace RogueIslands.Autofac
{
    public class ProjectLifetimeScope : AutofacScope
    {
        [SerializeField] private DebugLogManager _debugConsole;

        protected override void Configure(ContainerBuilder builder)
        {
            foreach (var instance in ModuleFinder.GetProjectModules())
            {
                builder.RegisterModule(instance);
            }

            builder.Register(_ => Instantiate(_debugConsole))
                .AutoActivate()
                .SingleInstance();
        }
    }
}