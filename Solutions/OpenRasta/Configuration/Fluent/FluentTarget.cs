namespace OpenRasta.Configuration.Fluent
{
    #region Using Directives

    using OpenRasta.Configuration.MetaModel;
    using OpenRasta.Contracts.Configuration.Fluent;
    using OpenRasta.Contracts.Configuration.MetaModel;
    using OpenRasta.Contracts.DI;
    using OpenRasta.Contracts.TypeSystem;
    using OpenRasta.DI;

    #endregion

    public class FluentTarget : IHas, IUses, IFluentTarget
    {
        private readonly IMetaModelRepository repository;
        private readonly IDependencyResolver resolver;

        public FluentTarget(IDependencyResolver resolver, IMetaModelRepository repository)
        {
            this.resolver = resolver;
            this.repository = repository;
        }

        public FluentTarget() : this(DependencyManager.GetService<IDependencyResolver>(), DependencyManager.GetService<IMetaModelRepository>())
        {
        }

        public IMetaModelRepository Repository
        {
            get { return this.repository; }
        }

        public IDependencyResolver Resolver
        {
            get { return this.resolver; }
        }

        public ITypeSystem TypeSystem
        {
            get { return this.resolver.Resolve<ITypeSystem>(); }
        }
    }
}