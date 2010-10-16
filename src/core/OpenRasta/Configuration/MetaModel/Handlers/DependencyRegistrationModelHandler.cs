namespace OpenRasta.Configuration.MetaModel.Handlers
{
    using System.Linq;

    using OpenRasta.DI;

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