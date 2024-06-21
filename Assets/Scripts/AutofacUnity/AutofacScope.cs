using Autofac;
using UnityEngine;

namespace AutofacUnity
{
    public class AutofacScope : MonoBehaviour
    {
        [SerializeField] private bool _autoRun = true;

        public bool AutoRun => _autoRun;

        public ILifetimeScope Container { get; private set; }

        public bool IsRoot => ReferenceEquals(Container.Tag, "root");

        private void Awake()
        {
            if (AutoRun)
                Build();
        }

        public void Build()
        {
            if (Container != null)
                return;

            if (AutofacSettings.Instance == null)
                AutofacSettings.LoadInstanceFromResources();

            if (AutofacSettings.Instance == null || AutofacSettings.Instance.RootScope == this)
            {
                var builder = new ContainerBuilder();
                Configure(builder);
                Container = builder.Build();
            }
            else
            {
                if(AutofacSettings.Instance.RootScope.Container == null)
                    AutofacSettings.Instance.RootScope.Build();
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