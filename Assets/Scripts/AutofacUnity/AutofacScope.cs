using Autofac;
using UnityEngine;

namespace AutofacUnity
{
    public class AutofacScope : MonoBehaviour
    {
        [SerializeField] private bool _autoRun = true;

        private ILifetimeScope _scope;

        public bool AutoRun => _autoRun;

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

            if (IsRoot || AutofacSettings.Instance.RootScope == null)
            {
                var builder = new ContainerBuilder();
                Configure(builder);
                Container = builder.Build();
            }
            else
            {
                Container = AutofacSettings.Instance.RootScope.Container.BeginLifetimeScope(this, Configure);
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