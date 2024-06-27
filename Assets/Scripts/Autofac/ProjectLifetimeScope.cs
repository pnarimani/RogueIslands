using Autofac;
using AutofacUnity;
using IngameDebugConsole;
using RogueIslands.Autofac.Modules;
using UnityEngine;

namespace RogueIslands.Autofac
{
    public class ProjectLifetimeScope : AutofacScope
    {
        [SerializeField] private DebugLogManager _debugConsole;

        protected override void Configure(ContainerBuilder builder)
        {
            builder.RegisterModule<AudioFmodModule>();
            builder.RegisterModule<YamlSerializationModule>();

            builder.Register(_ => Instantiate(_debugConsole))
                .AutoActivate()
                .SingleInstance();
        }
    }
}