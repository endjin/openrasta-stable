namespace OpenRasta.DI.Ninject
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::Ninject;
    using global::Ninject.Activation;
    using global::Ninject.Activation.Caching;
    using global::Ninject.Parameters;
    using global::Ninject.Planning;
    using global::Ninject.Planning.Bindings;
    using global::Ninject.Selection;
    using global::Ninject.Selection.Heuristics;

    using OpenRasta.Contracts.DI;
    using OpenRasta.Contracts.Pipeline;
    using OpenRasta.DI.Internal;
    using OpenRasta.Exceptions;
    using OpenRasta.Extensions;

    using IPipeline = global::Ninject.Activation.IPipeline;
    using NinjectBinding = global::Ninject.Planning.Bindings.Binding;

    #endregion

    /// <summary>
    /// A Ninject-based <see cref="IDependencyResolver"/>.
    /// </summary>
    public class NinjectDependencyResolver : DependencyResolverCore, IDependencyResolver
    {
        private static readonly IEnumerable<IParameter> EmptyParameters = new IParameter[] { };

        private readonly IKernel kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectDependencyResolver"/> class.
        /// </summary>
        public NinjectDependencyResolver() : this(null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectDependencyResolver"/> class.
        /// </summary>
        /// <param name="kernel">The kernel to use.</param>
        public NinjectDependencyResolver(IKernel kernel)
        {
            this.kernel = kernel ?? CreateKernel();
        }

        /// <summary>
        /// Creates an <see cref="IKernel"/> that is configured in the way OpenRasta expects.
        /// </summary>
        /// <remarks>
        /// OpenRasta is written with some implicit assumptions or requirements about how the
        /// IoC container will work. For example, which constructor is selected for injection
        /// or the fact that public "settable" properties will be injected if possible
        /// and left alone if not possible.
        /// </remarks>
        /// <returns>A new <see cref="IKernel"/></returns>
        public static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            ConfigureKernel(kernel);
            return kernel;
        }

        /// <summary>
        /// Creates an <see cref="IKernel"/> that is configured in the way OpenRasta expects,
        /// using the specified "parent kernel".
        /// </summary>
        /// <remarks>
        /// OpenRasta is written with some implicit assumptions or requirements about how the
        /// IoC container will work. For example, which constructor is selected for injection
        /// or the fact that public "settable" properties will be injected if possible
        /// and left alone if not possible.
        /// 
        /// If a request to the kernel is not resolvable, the kernel will attempt to resolve the type
        /// from the <see param="parentKernel" />.
        /// </remarks>
        /// <returns>A new <see cref="IKernel"/></returns>
        public static IKernel CreateKernel(IKernel parentKernel)
        {
            var kernel = new SubContainerKernel(parentKernel);
            ConfigureKernel(kernel);
            return kernel;
        }

        public void Destruct(string key, object instance)
        {
            var store = this.GetStore();
            store[key] = null;
        }

        /// <summary>
        /// Adds the dependency.
        /// </summary>
        /// <param name="concreteType">Type of the concrete class to create.</param>
        /// <param name="lifetime">The lifetime of the registration.</param>
        protected override void AddDependencyCore(Type concreteType, DependencyLifetime lifetime)
        {
            this.AddDependencyCore(concreteType, concreteType, lifetime);
        }

        /// <summary>
        /// Adds the dependency.
        /// </summary>
        /// <param name="serviceType">Type of the service to bind to.</param>
        /// <param name="concreteType">Type of the concrete class to create.</param>
        /// <param name="lifetime">The lifetime of the registration.</param>
        protected override void AddDependencyCore(Type serviceType, Type concreteType, DependencyLifetime lifetime)
        {
            var binding = CreateBinding(serviceType, lifetime);

            if (lifetime == DependencyLifetime.PerRequest)
            {
                binding.ProviderCallback = ctx => new PerRequestProvider(concreteType, ctx.Kernel.Components.Get<IPlanner>(), ctx.Kernel.Components.Get<ISelector>());
                binding.Target = BindingTarget.Provider;
            }
            else
            {
                var bindingBuilder = new BindingBuilder<object>(binding);
                var bindingScope = bindingBuilder.To(concreteType);
                
                if (lifetime == DependencyLifetime.Singleton)
                {
                    bindingScope.InSingletonScope();
                }
            }

            this.kernel.AddBinding(binding);
        }

        /// <summary>
        /// Adds the an instance to the dependencies.
        /// </summary>
        /// <param name="serviceType">Type of the service to add.</param>
        /// <param name="instance">The instance of the service to add.</param>
        /// <param name="lifetime">The lifetime for the registration.</param>
        protected override void AddDependencyInstanceCore(Type serviceType, object instance, DependencyLifetime lifetime)
        {
            if (lifetime == DependencyLifetime.Transient) return;

            var binding = this.kernel.GetBindings(serviceType).FirstOrDefault();
            bool foundExistingBinding = binding != null;
            
            if (binding == null)
            {
                binding = CreateBinding(serviceType, lifetime);
                this.kernel.AddBinding(binding);
            }

            var builder = new BindingBuilder<object>(binding);
            
            if (lifetime == DependencyLifetime.PerRequest)
            {
                if (foundExistingBinding && binding.Target != BindingTarget.Method)
                {
                    // A binding exists, but wasn't specified as an instance callback. Error!
                    throw new DependencyResolutionException("Cannot register an instance for a type already registered");
                }

                var store = this.GetStore();
                var key = serviceType.GetKey();
                store[key] = instance;

                if (!foundExistingBinding)
                {
                    store.GetContextInstances().Add(new ContextStoreDependency(key, instance, new ContextStoreDependencyCleaner(this.kernel)));
                }

                builder.ToMethod(c =>
                                     {
                                         var ctxStore = GetStore();
                                         return ctxStore[serviceType.GetKey()];
                                     });
            }
            else if (lifetime == DependencyLifetime.Singleton)
            {
                builder.ToConstant(instance).InSingletonScope();
            }
        }

        /// <summary>
        /// Resolves all the specified types.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns></returns>
        protected override IEnumerable<TService> ResolveAllCore<TService>()
        {
            return this.kernel.GetAll<TService>();
        }

        /// <summary>
        /// Resolves an instance of the <see cref="IKernel"/>.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns></returns>
        protected override object ResolveCore(Type serviceType)
        {
            this.RequireDependancy(serviceType);

            return this.kernel.Get(serviceType);
        }

        /// <summary>
        /// Determines whether the specified service type has dependency.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>
        ///     <see langword="true"/> if the specified service type has dependency; otherwise, <see langword="false"/>.
        /// </returns>
        public bool HasDependency(Type serviceType)
        {
            if (serviceType == null)
            {
                return false;
            }

            var bindings = this.GetBindings(serviceType);
            
            return bindings.Any();
        }

        /// <summary>
        /// Determines whether a binding exists between the specified service and concrete types.
        /// </summary>
        public bool HasDependencyImplementation(Type serviceType, Type concreteType)
        {
            if (serviceType == null || concreteType == null)
            {
                return false;
            }

            if (serviceType == concreteType)
            {
                return this.HasDependency(serviceType);
            }

            var bindings = this.GetBindings(serviceType);
            var request = this.kernel.CreateRequest(serviceType, null, EmptyParameters, false);
            
            return bindings.Any(b =>
                {
                    if (b.Target != BindingTarget.Type)
                    {
                        return false;
                    }

                    var context = new Context(
                        this.kernel,
                        request,
                        b,
                        this.kernel.Components.Get<ICache>(),
                        this.kernel.Components.Get<IPlanner>(),
                        this.kernel.Components.Get<IPipeline>());
                        return b.GetProvider(context).Type == concreteType;
                });
        }

        public void HandleIncomingRequestProcessed()
        {
            var store = this.GetStore();
            store.Destruct();
        }

        private static bool IsWebInstance(IBinding binding)
        {
            return (binding is WebBinding) && (binding.Target == BindingTarget.Method);
        }

        private static void ConfigureKernel(IKernel kernel)
        {
            // Needed to support OpenRasta's assumptions.
            kernel.Components.Add<IInjectionHeuristic, AllResolvablePropertiesInjectionHeuristic>();
            kernel.Components.RemoveAll(typeof(IConstructorScorer));
            kernel.Components.Add<IConstructorScorer, InjectableConstructorScorer>();
        }

        private static IBinding CreateBinding(Type serviceType, DependencyLifetime lifetime)
        {
            return (lifetime == DependencyLifetime.PerRequest)
                       ? new WebBinding(serviceType)
                       : new NinjectBinding(serviceType);
        }

        private void RequireDependancy(Type serviceType)
        {
            if (!this.HasDependency(serviceType))
            {
                throw new DependencyResolutionException("Unable to resolve dependency for {0}".With(serviceType));
            }
        }

        private IEnumerable<IBinding> GetBindings(Type service)
        {
            return from binding in this.kernel.GetBindings(service)
                   where this.IsAvailable(binding)
                   select binding;
        }

        private bool IsAvailable(IBinding binding)
        {
            if (IsWebInstance(binding))
            {
                if (!this.HasDependency(typeof(IContextStore)))
                {
                    return false;
                }

                var store = this.GetStore();
                bool instanceAvailable = store[binding.Service.GetKey()] != null;
                return instanceAvailable;
            }

            return binding.Target != BindingTarget.Method;
        }

        private IContextStore GetStore()
        {
            return this.kernel.Get<IContextStore>();
        }
    }
}