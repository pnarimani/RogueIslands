using Autofac;
using IngameDebugConsole;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RogueIslands.DependencyInjection.Autofac
{
    public class ProjectLifetimeScope : StaticallyResolvableLifetimeScope
    {
        [SerializeField] private DebugLogManager _debugConsole;
        
        protected override void Configure(ContainerBuilder builder)
        {
            foreach (var instance in ModuleFinder.GetProjectModules())
            {
                instance.Load(builder);
            }

            builder.Register(_ =>
                {
                    var debugLogManager = Instantiate(_debugConsole);
#if UNITY_EDITOR
                    SceneVisibilityManager.instance.DisablePicking(debugLogManager.gameObject, true);
#endif
                    return debugLogManager;
                })
                .AutoActivate()
                .SingleInstance();
        }
    }
}