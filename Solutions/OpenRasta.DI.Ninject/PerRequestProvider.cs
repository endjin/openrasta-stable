namespace OpenRasta.DI.Ninject
{
    #region Using Directives

    using System;

    using global::Ninject;
    using global::Ninject.Activation;
    using global::Ninject.Activation.Providers;
    using global::Ninject.Planning;
    using global::Ninject.Selection;
    using global::Ninject.Syntax;

    using OpenRasta.Contracts.Pipeline;
    using OpenRasta.DI.Internal;

    #endregion

    /// <summary>
    /// A Ninject provider that resolves/caches instances on a OpenRasta PerRequest basis
    /// using <see cref="IContextStore"/>.
    /// </summary>
    public class PerRequestProvider : StandardProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PerRequestProvider"/> class.
        /// </summary>
        /// <param name="type">The type (or prototype) of instances the provider creates.</param>
        /// <param name="planner">The <see cref="IPlanner"/> component.</param>
        /// <param name="selector">The <see cref="ISelector"/> component</param>
        public PerRequestProvider(Type type, IPlanner planner, ISelector selector)
            : base(type, planner, selector)
        {
        }

        /// <summary>
        /// Creates an instance within the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The created instance.</returns>
        public override object Create(IContext context)
        {
            var store = GetStore(context.Kernel);
            string key = context.Request.Service.GetKey();

            if (store[key] != null)
            {
                return store[key];
            }

            var instance = base.Create(context);
            store[key] = instance;

            store.GetContextInstances().Add(new ContextStoreDependency(
                key, 
                instance, 
                new ContextStoreDependencyCleaner(context.Kernel)));

            return store[key];
        }

        private static IContextStore GetStore(IResolutionRoot kernel)
        {
            var store = kernel.TryGet<IContextStore>();
            
            if (store == null)
            {
                throw new InvalidOperationException(
                    "There is no IContextStore implementation registered in the container.");
            }

            return store;
        }
    }
}