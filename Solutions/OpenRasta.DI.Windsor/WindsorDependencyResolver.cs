namespace OpenRasta.DI.Windsor
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Castle.Core;
    using Castle.MicroKernel;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;

    using OpenRasta.Contracts.Pipeline;
    using OpenRasta.DI.Internal;
    using OpenRasta.Exceptions;

    #endregion

    public class WindsorDependencyResolver : DependencyResolverCore, Contracts.DI.IDependencyResolver
    {
        private readonly IWindsorContainer windsorContainer;

        public WindsorDependencyResolver(IWindsorContainer container)
        {
            this.windsorContainer = container;
        }

        public bool HasDependency(Type serviceType)
        {
            if (serviceType == null)
            {
                return false;
            }

            return this.AvailableHandlers(this.windsorContainer.Kernel.GetHandlers(serviceType)).Any();
        }

        public bool HasDependencyImplementation(Type serviceType, Type concreteType)
        {
            return
                this.AvailableHandlers(this.windsorContainer.Kernel.GetHandlers(serviceType))
                    .Any(h => h.ComponentModel.Implementation == concreteType);
        }

        public void HandleIncomingRequestProcessed()
        {
            var store = this.windsorContainer.Resolve<IContextStore>();

            store.Destruct();
        }

        protected override object ResolveCore(Type serviceType)
        {
            return this.windsorContainer.Resolve(serviceType);
        }

        protected override IEnumerable<TService> ResolveAllCore<TService>()
        {
            // return _windsorContainer.ResolveAll<TService>();
            var handlers = this.windsorContainer.Kernel.GetAssignableHandlers(typeof(TService));
            var resolved = new List<TService>();
            
            foreach (var handler in this.AvailableHandlers(handlers))
            {
                try
                {
                    resolved.Add((TService)this.windsorContainer.Resolve(handler.ComponentModel.Name, typeof(TService)));
                }
                catch
                {
                    continue;
                }
            }
            
            return resolved;
        }

        protected override void AddDependencyCore(Type dependent, Type concrete, DependencyLifetime lifetime)
        {
            string componentName = Guid.NewGuid().ToString();

            if (lifetime != DependencyLifetime.PerRequest)
            {
                this.windsorContainer.Register(
                    Component.For(dependent)
                             .Named(componentName)
                             .ImplementedBy(concrete)
                             .LifeStyle.Is(ConvertLifestyles.ToLifestyleType(lifetime)));
            }
            else
            {
                this.windsorContainer.Register(
                    Component.For(dependent)
                             .Named(componentName)
                             .ImplementedBy(concrete).LifeStyle.Custom(typeof(ContextStoreLifetime)));
            }
        }

        protected override void AddDependencyInstanceCore(Type serviceType, object instance, DependencyLifetime lifetime)
        {
            string key = Guid.NewGuid().ToString();

            if (lifetime == DependencyLifetime.PerRequest)
            {
                // try to see if we have a registration already
                var store = (IContextStore)Resolve(typeof(IContextStore));

                if (this.windsorContainer.Kernel.HasComponent(serviceType))
                {
                    var handler = this.windsorContainer.Kernel.GetHandler(serviceType);

                    if (handler.ComponentModel.ExtendedProperties[Constants.RegIsInstanceKey] != null)
                    {
                        // if there's already an instance registration we update the store with the correct reg.
                        store[handler.ComponentModel.Name] = instance;
                    }
                    else
                    {
                        throw new DependencyResolutionException("Cannot register an instance for a type already registered");
                    }
                }
                else
                {
                    var component = new ComponentModel(key, serviceType, instance.GetType());
                    var customLifestyle = typeof(ContextStoreLifetime);

                    component.LifestyleType = LifestyleType.Custom;
                    component.CustomLifestyle = customLifestyle;
                    component.CustomComponentActivator = typeof(ContextStoreInstanceActivator);
                    component.ExtendedProperties[Constants.RegIsInstanceKey] = true;
                    component.Name = component.Name;
                    
                    this.windsorContainer.Kernel.Register(Component.For(component));

                    store[component.Name] = instance;
                }
            }
            else if (lifetime == DependencyLifetime.Singleton)
            {
                this.windsorContainer.Kernel.Register(
                    Component.For(serviceType)
                             .Named(key)
                             .Instance(instance));
            }
        }

        protected override void AddDependencyCore(Type handlerType, DependencyLifetime lifetime)
        {
            this.AddDependencyCore(handlerType, handlerType, lifetime);
        }

        private static bool IsWebInstance(ComponentModel component)
        {
            return typeof(ContextStoreLifetime).IsAssignableFrom(component.CustomLifestyle) &&
                   component.ExtendedProperties[Constants.RegIsInstanceKey] != null;
        }

        private IEnumerable<IHandler> AvailableHandlers(IEnumerable<IHandler> handlers)
        {
            return from handler in handlers
                   where handler.CurrentState == HandlerState.Valid
                         && this.IsAvailable(handler.ComponentModel)
                   select handler;
        }

        private bool IsAvailable(ComponentModel component)
        {
            bool webInstance = IsWebInstance(component);

            if (webInstance)
            {
                if (component.Name == null || !this.HasDependency(typeof(IContextStore)))
                {
                    return false;
                }

                var store = this.windsorContainer.Resolve<IContextStore>();
                bool instanceAvailable = store[component.Name] != null;
                
                return instanceAvailable;
            }

            return true;
        }
    }
}