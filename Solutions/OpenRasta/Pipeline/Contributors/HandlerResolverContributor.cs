namespace OpenRasta.Pipeline.Contributors
{
    #region Using Directives

    using System.Linq;

    using OpenRasta.Contracts.DI;
    using OpenRasta.Contracts.Handlers;
    using OpenRasta.Contracts.Pipeline;
    using OpenRasta.Contracts.Web;

    #endregion

    /// <summary>
    /// Resolves the handler attached to a resource type.
    /// </summary>
    public class HandlerResolverContributor : KnownStages.IHandlerSelection
    {
        private readonly IDependencyResolver resolver;
        private readonly IHandlerRepository handlers;

        public HandlerResolverContributor(IDependencyResolver resolver, IHandlerRepository repository)
        {
            this.resolver = resolver;
            this.handlers = repository;
        }

        public void Initialize(IPipeline pipelineRunner)
        {
            pipelineRunner.Notify(this.ResolveHandler).After<KnownStages.IUriMatching>();
        }

        public PipelineContinuation ResolveHandler(ICommunicationContext context)
        {
            var handlerTypes = this.handlers.GetHandlerTypesFor(context.PipelineData.ResourceKey);

            if (handlerTypes != null && handlerTypes.Count() > 0)
            {
                context.PipelineData.SelectedHandlers = handlerTypes.ToList();
                return PipelineContinuation.Continue;
            }

            return PipelineContinuation.Abort;
        }
    }
}