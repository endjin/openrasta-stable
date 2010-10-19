namespace OpenRasta.Configuration.MetaModel.Handlers
{
    #region Using Directives

    using System.Linq;

    using OpenRasta.Contracts.Configuration.MetaModel;
    using OpenRasta.Contracts.DI;

    #endregion

    public class DependencyRegistrationMetaModelHandler : AbstractMetaModelHandler
    {
        private readonly IDependencyResolver resolver;

        public DependencyRegistrationMetaModelHandler(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public override void PreProcess(IMetaModelRepository repository)
        {
            foreach (var model in repository.CustomRegistrations.OfType<DependencyRegistrationModel>())
            {
                this.resolver.AddDependency(model.ServiceType, model.ConcreteType, model.Lifetime);
            }
        }
    }
}