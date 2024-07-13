using System;
using Autofac;
using UnityEngine;

namespace AutofacUnity
{
    [DefaultExecutionOrder(-99999)]
    public class AutofacScope : MonoBehaviour
    {
        [SerializeField] private bool _autoRun = true;

        private bool _isBuilding;
        
        public bool AutoRun
        {
            get => _autoRun;
            internal set => _autoRun = value;
        }

        public ILifetimeScope Container { get; private set; }

        public bool IsRoot { get; internal set; }

        private void Awake()
        {
            if (AutoRun)
                Build();
        }

        public void Build()
        {
            if (Container != null)
                return;

            if (_isBuilding)
                throw new InvalidOperationException("Cannot access container while it's building");
            _isBuilding = true;

            if (AutofacSettings.Instance == null)
                AutofacSettings.LoadInstanceFromResources();

            if (IsRoot || AutofacSettings.Instance == null || !AutofacSettings.Instance.HasRootScope())
            {
                var builder = new ContainerBuilder();
                Configure(builder);
                Container = builder.Build();
            }
            else
            {
                if(AutofacSettings.Instance.RootScope == null)
                    AutofacSettings.Instance.InitializeRootScope();
                Container = AutofacSettings.Instance.RootScope.Container!.BeginLifetimeScope(this, Configure);
            }
        }

        protected virtual void Configure(ContainerBuilder builder)
        {
        }

        private void OnDestroy()
        {
            Container?.Dispose();
            Container = null;
        }
    }
}