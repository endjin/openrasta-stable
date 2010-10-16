namespace OpenRasta.Configuration.MetaModel.Handlers
{
    using OpenRasta.Handlers;

    public class HandlerMetaModelHandler : AbstractMetaModelHandler
    {
        private readonly IHandlerRepository repository;

        public HandlerMetaModelHandler(IHandlerRepository repository)
        {
            this.repository = repository;
        }

        public override void Process(IMetaModelRepository repository)
        {
            foreach (var resource in repository.ResourceRegistrations)
            {
                foreach (var handler in resource.Handlers)
                {
                    this.repository.AddResourceHandler(resource.ResourceKey, handler);
                }
            }
        }
    }
}