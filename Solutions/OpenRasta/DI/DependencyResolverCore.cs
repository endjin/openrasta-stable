namespace OpenRasta.DI
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using OpenRasta.Exceptions;
    using OpenRasta.Extensions;

    #endregion

    public abstract class DependencyResolverCore
    {
        public void AddDependency(Type serviceType, Type concreteType, DependencyLifetime lifetime)
        {
            CheckConcreteType(concreteType);
            CheckServiceType(serviceType, concreteType);
            CheckLifetime(lifetime);
            this.AddDependencyCore(serviceType, concreteType, lifetime);
        }

        public void AddDependency(Type concreteType, DependencyLifetime lifetime)
        {
            CheckConcreteType(concreteType);
            CheckLifetime(lifetime);

            this.AddDependencyCore(concreteType, lifetime);
        }

        public void AddDependencyInstance(Type serviceType, object instance, DependencyLifetime lifetime)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (lifetime == DependencyLifetime.Transient)
            {
                throw new ArgumentException("Cannot register an instance for Transient lifetimes.", "lifetime");
            }

            CheckServiceType(serviceType, instance.GetType());
            this.AddDependencyInstanceCore(serviceType, instance, lifetime);
        }

        public object Resolve(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            try
            {
                return this.ResolveCore(serviceType);
            }
            catch (Exception e)
            {
                if (e is DependencyResolutionException)
                {
                    throw;
                }

                throw new DependencyResolutionException(
                    "An error occurred while trying to resolve type {0}.".With(serviceType.Name), e);
            }
        }

        public IEnumerable<TService> ResolveAll<TService>()
        {
            try
            {
                return ResolveAllCore<TService>();
            }
            catch (Exception e)
            {
                if (e is DependencyResolutionException)
                {
                    throw;
                }

                throw new DependencyResolutionException(
                    "An error occurred while trying to resolve type {0}.".With(typeof(TService).Name), e);
            }
        }

        protected static void CheckConcreteType(Type concreteType)
        {
            if (concreteType == null)
            {
                throw new ArgumentNullException("concreteType");
            }

            if (concreteType.IsAbstract)
            {
                throw new InvalidOperationException(
                    "The type {0} is abstract. You cannot register an abstract type for initialization.".With(
                        concreteType.FullName));
            }
        }

        protected static void CheckLifetime(DependencyLifetime lifetime)
        {
            if (!Enum.IsDefined(typeof(DependencyLifetime), lifetime))
            {
                throw new InvalidOperationException(
                    string.Format("Value {0} is unknown for enumeration DependencyLifetime.", lifetime));
            }
        }

        protected static void CheckServiceType(Type serviceType, Type concreteType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            if (concreteType == null)
            {
                throw new ArgumentNullException("concreteType");
            }

            if (!serviceType.IsAssignableFrom(concreteType))
            {
                throw new InvalidOperationException(
                    "The type {0} doesn't implement or inherit from {1}.".With(concreteType.Name, serviceType.Name));
            }
        }

        protected abstract void AddDependencyCore(Type serviceType, Type concreteType, DependencyLifetime lifetime);

        protected abstract void AddDependencyCore(Type concreteType, DependencyLifetime lifetime);

        protected abstract void AddDependencyInstanceCore(Type serviceType, object instance, DependencyLifetime lifetime);

        protected abstract IEnumerable<TService> ResolveAllCore<TService>();

        protected abstract object ResolveCore(Type serviceType);
    }
}