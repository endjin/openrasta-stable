namespace OpenRasta.Contracts.DI
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using OpenRasta.DI;

    #endregion

    public interface IDependencyResolver
    {
        bool HasDependency(Type serviceType);

        bool HasDependencyImplementation(Type serviceType, Type concreteType);

        void AddDependency(Type concreteType, DependencyLifetime lifetime);

        void AddDependency(Type serviceType, Type concreteType, DependencyLifetime dependencyLifetime);

        void AddDependencyInstance(Type registeredType, object value, DependencyLifetime dependencyLifetime);

        IEnumerable<TService> ResolveAll<TService>();

        object Resolve(Type type);

        void HandleIncomingRequestProcessed();
    }
}