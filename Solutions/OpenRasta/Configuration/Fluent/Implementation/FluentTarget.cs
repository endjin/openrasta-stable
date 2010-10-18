namespace OpenRasta.Configuration.Fluent.Implementation
{
    using OpenRasta.Configuration.MetaModel;
    using OpenRasta.DI;
    using OpenRasta.TypeSystem;

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