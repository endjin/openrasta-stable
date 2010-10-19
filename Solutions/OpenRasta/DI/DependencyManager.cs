namespace OpenRasta.DI
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using OpenRasta.Configuration.Registration;
    using OpenRasta.Contracts.Codecs;
    using OpenRasta.Contracts.DI;
    using OpenRasta.Contracts.Handlers;
    using OpenRasta.Contracts.Pipeline;
    using OpenRasta.Contracts.Web;
    using OpenRasta.Exceptions;

    #endregion

    /// <summary>
    /// Provides easy access to common services and dependency-specific properties.
    /// </summary>
    public static class DependencyManager
    {
        [ThreadStatic]
        private static Stack<IDependencyResolver> backupResolvers;

        [ThreadStatic]
        private static IDependencyResolver resolver;

        static DependencyManager()
        {
            AutoRegisterDependencies = true;
        }

        /// <summary>
        /// Gets or sets a value defining if unregistered dependencies resolved through a call to <see cref="GetService"/> 
        /// are automatically registered in the container.
        /// </summary>
        /// <remarks>This covers user-specified codecs, handlers and any type provided to the <see cref="GetService"/> method.
        /// <c>true</c> by default.</remarks>
        public static bool AutoRegisterDependencies { get; set; }

        public static ICodecRepository Codecs
        {
            get { return GetService<ICodecRepository>(); }
        }

        public static IHandlerRepository Handlers
        {
            get { return GetService<IHandlerRepository>(); }
        }

        public static bool IsAvailable
        {
            get { return resolver != null; }
        }

        public static IPipeline Pipeline
        {
            get { return GetService<IPipeline>(); }
        }

        public static IUriResolver Uris
        {
            get { return GetService<IUriResolver>(); }
        }

        public static T GetService<T>() where T : class
        {
            return (T)GetService(typeof(T));
        }

        /// <summary>
        /// Resolve a component, optionally registering it in the container if <see cref="AutoRegisterDependencies"/> is set to <c>true</c>.
        /// </summary>
        /// <param name="dependencyType"></param>
        /// <returns></returns>
        public static object GetService(Type dependencyType)
        {
            if (dependencyType == null)
            {
                return null;
            }

            if (resolver == null)
            {
                throw new DependencyResolutionException(
                    "Cannot resolve services when no _resolver has been configured.");
            }

            if (AutoRegisterDependencies && !dependencyType.IsAbstract)
            {
                if (!resolver.HasDependency(dependencyType))
                {
                    resolver.AddDependency(dependencyType, DependencyLifetime.Transient);
                }
            }

            return resolver.Resolve(dependencyType);
        }

        /// <summary>
        /// Set a dependency resolver for the current thread
        /// </summary>
        /// <param name="resolver">An instance of a dependency resolver.</param>
        /// <remarks>If no dependency registrar is registered in the container, the <see cref="DefaultDependencyRegistrar"/> will be used instead.</remarks>
        public static void SetResolver(IDependencyResolver resolver)
        {
            if (DependencyManager.resolver != null)
            {
                if (backupResolvers == null)
                {
                    backupResolvers = new Stack<IDependencyResolver>();
                }

                backupResolvers.Push(DependencyManager.resolver);
            }

            DependencyManager.resolver = resolver;
        }

        public static void UnsetResolver()
        {
            resolver = backupResolvers != null && backupResolvers.Count > 0 ? backupResolvers.Pop() : null;
        }
    }
}