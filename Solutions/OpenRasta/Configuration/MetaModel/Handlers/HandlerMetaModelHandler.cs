namespace OpenRasta.Configuration.MetaModel.Handlers
{
    using OpenRasta.Contracts.Configuration.MetaModel;
    using OpenRasta.Contracts.Handlers;

    public class HandlerMetaModelHandler : AbstractMetaModelHandler
    {
        private readonly IHandlerRepository handlerRepository;

        public HandlerMetaModelHandler(IHandlerRepository handlerRepository)
        {
            this.handlerRepository = handlerRepository;
        }

        public override void Process(IMetaModelRepository repository)
        {
            foreach (var resource in repository.ResourceRegistrations)
            {
                foreach (var handler in resource.Handlers)
                {
                    this.handlerRepository.AddResourceHandler(resource.ResourceKey, handler);
                }
            }
        }
    }
}