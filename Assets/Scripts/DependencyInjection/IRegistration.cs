using System;

namespace RogueIslands.DependencyInjection
{
    public interface IRegistration<TImplementer>
    {
        IRegistration<TImplementer> As<T>();
        IRegistration<TImplementer> AsImplementedInterfaces();
        IRegistration<TImplementer> AsSelf();
        IRegistration<TImplementer> AutoActivate();
        IRegistration<TImplementer> SingleInstance();
        IRegistration<TImplementer> OnActivated(Action<IContainer, TImplementer> onActivated);
        IRegistration<TImplementer> InstancePerDependency();
    }
}