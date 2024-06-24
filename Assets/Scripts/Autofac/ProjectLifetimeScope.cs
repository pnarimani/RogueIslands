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
            builder.RegisterModule<AudioFmodModule>();

            builder.Register(_ => Instantiate(_debugConsole))
                .AutoActivate()
                .SingleInstance();
        }
    }
}