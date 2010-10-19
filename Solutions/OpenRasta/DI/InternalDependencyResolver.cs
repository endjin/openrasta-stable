namespace OpenRasta.DI
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OpenRasta.Contracts.DI;
    using OpenRasta.Contracts.Diagnostics;
    using OpenRasta.Contracts.Pipeline;
    using OpenRasta.DI.Internal;
    using OpenRasta.Diagnostics;
    using OpenRasta.Exceptions;
    using OpenRasta.Extensions; 

    #endregion;

    public class InternalDependencyResolver : DependencyResolverCore, IDependencyResolver
    {
        private readonly Dictionary<DependencyLifetime, DependencyLifetimeManager> lifetimeManagers;

        private ILogger log = new TraceSourceLogger();

        public InternalDependencyResolver()
        {
            this.Registrations = new DependencyRegistrationCollection();
            this.lifetimeManagers = new Dictionary<DependencyLifetime, DependencyLifetimeManager>
            {
                { DependencyLifetime.Transient, new TransientLifetimeManager(this) }, 
                { DependencyLifetime.Singleton, new SingletonLifetimeManager(this) }, 
                { DependencyLifetime.PerRequest, new PerRequestLifetimeManager(this) }
            };
        }

        public ILogger Log
        {
            get { return this.log; }
            set { this.log = value; }
        }

        public DependencyRegistrationCollection Registrations { get; private set; }

        protected override void AddDependencyCore(Type serviceType, Type concreteType, DependencyLifetime lifetime)
        {
            this.Registrations.Add(new DependencyRegistration(serviceType, concreteType, this.lifetimeManagers[lifetime]));
        }

        protected override void AddDependencyCore(Type concreteType, DependencyLifetime lifetime)
        {
            this.AddDependencyCore(concreteType, concreteType, lifetime);
        }

        protected override void AddDependencyInstanceCore(Type serviceType, object instance, DependencyLifetime lifetime)
        {
            var instanceType = instance.GetType();

            var registration = new DependencyRegistration(serviceType, instanceType, this.lifetimeManagers[lifetime], instance);

            this.Registrations.Add(registration);
        }

        protected override IEnumerable<TService> ResolveAllCore<TService>()
        {
            return from dependency in this.Registrations[typeof(TService)]
                   where dependency.LifetimeManager.IsRegistrationAvailable(dependency)
                   select (TService)this.Resolve(dependency);
        }

        protected override object ResolveCore(Type serviceType)
        {
            if (!this.Registrations.HasRegistrationForService(serviceType))
            {
                throw new DependencyResolutionException("No type registered for {0}".With(serviceType.Name));
            }

            return this.Resolve(this.Registrations.GetRegistrationForService(serviceType));
        }

        public void HandleIncomingRequestProcessed()
        {
            var store = (IContextStore)this.Resolve(typeof(IContextStore));
            store.Destruct();
        }

        public bool HasDependency(Type serviceType)
        {
            if (serviceType == null)
            {
                return false;
            }

            return this.Registrations.HasRegistrationForService(serviceType);
        }

        public bool HasDependencyImplementation(Type serviceType, Type concreteType)
        {
            return this.Registrations.HasRegistrationForService(serviceType) && this.Registrations[serviceType].Count(r => r.ConcreteType == concreteType) >= 1;
        }

        private object Resolve(DependencyRegistration dependency)
        {
            var context = new ResolveContext(this.Registrations, this.Log);
            
            return context.Resolve(dependency);
        }
    }
}