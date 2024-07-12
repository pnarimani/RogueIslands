using System;
using Autofac;
using Autofac.Builder;

namespace RogueIslands.DependencyInjection.Autofac
{
    public class RegistrationProxy<TImplementer, TActivatorData> : IRegistration<TImplementer>
    {
        private IRegistrationBuilder<TImplementer, TActivatorData, SingleRegistrationStyle> _inner;

        public RegistrationProxy(IRegistrationBuilder<TImplementer, TActivatorData, SingleRegistrationStyle> inner)
        {
            _inner = inner;
        }

        public IRegistration<TImplementer> As<T>()
        {
            _inner = _inner.As<T>();
            return this;
        }

        public IRegistration<TImplementer> AsImplementedInterfaces()
        {
            switch (_inner)
            {
                case IRegistrationBuilder<TImplementer, ConcreteReflectionActivatorData, SingleRegistrationStyle>
                    concrete:
                    concrete.AsImplementedInterfaces();
                    break;
                case IRegistrationBuilder<TImplementer, SimpleActivatorData, SingleRegistrationStyle> simple:
                    simple.AsImplementedInterfaces();
                    break;
                default:
                    throw new InvalidOperationException("This registration doesn't support AsImplementedInterfaces");
            }

            return this;
        }

        public IRegistration<TImplementer> AsSelf()
        {
            switch (_inner)
            {
                case IRegistrationBuilder<TImplementer, ConcreteReflectionActivatorData, SingleRegistrationStyle>
                    concrete:
                    concrete.AsSelf();
                    break;
                case IRegistrationBuilder<TImplementer, SimpleActivatorData, SingleRegistrationStyle> simple:
                    simple.AsSelf();
                    break;
                default:
                    throw new InvalidOperationException("This registration doesn't support AsSelf");
            }

            return this;
        }

        public IRegistration<TImplementer> AutoActivate()
        {
            _inner.AutoActivate();
            return this;
        }

        public IRegistration<TImplementer> SingleInstance()
        {
            _inner.SingleInstance();
            return this;
        }

        public IRegistration<TImplementer> OnActivated(Action<IContainer, TImplementer> onActivated)
        {
            _inner.OnActivated(e => onActivated(new ContainerProxy(e.Context), e.Instance));
            return this;
        }

        public IRegistration<TImplementer> InstancePerDependency()
        {
            _inner.InstancePerDependency();
            return this;
        }

        public IRegistration<TImplementer> IfNotRegistered(Type type)
        {
            _inner.IfNotRegistered(type);
            return this;
        }
    }
}