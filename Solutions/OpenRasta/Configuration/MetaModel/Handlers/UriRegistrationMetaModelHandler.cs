namespace OpenRasta.Configuration.MetaModel.Handlers
{
    using OpenRasta.Contracts.Web;
    using OpenRasta.Web;

    public class UriRegistrationMetaModelHandler : AbstractMetaModelHandler
    {
        private readonly IUriResolver uriResolver;

        public UriRegistrationMetaModelHandler(IUriResolver uriResolver)
        {
            this.uriResolver = uriResolver;
        }

        public override void Process(IMetaModelRepository repository)
        {
            foreach (var resource in repository.ResourceRegistrations)
            {
                foreach (var uriRegistration in resource.Uris)
                {
                    this.uriResolver.Add(
                        new UriRegistration(
                            uriRegistration.Uri, 
                            resource.ResourceKey, 
                            uriRegistration.Name, 
                            uriRegistration.Language));
                }
            }
        }
    }
}