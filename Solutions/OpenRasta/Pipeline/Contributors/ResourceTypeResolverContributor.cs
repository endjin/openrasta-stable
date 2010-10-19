namespace OpenRasta.Pipeline.Contributors
{
    #region Using Directives

    using OpenRasta.Contracts.Diagnostics;
    using OpenRasta.Contracts.Pipeline;
    using OpenRasta.Contracts.Web;
    using OpenRasta.Diagnostics;
    using OpenRasta.Extensions;
    using OpenRasta.Web;
    using OpenRasta.Web.Internal;

    #endregion

    public class ResourceTypeResolverContributor : KnownStages.IUriMatching
    {
        private readonly IUriResolver uriRepository;

        public ResourceTypeResolverContributor(IUriResolver uriRepository)
        {
            this.uriRepository = uriRepository;
            this.Log = NullLogger.Instance;
        }

        public ILogger Log { get; set; }

        public void Initialize(IPipeline pipelineRunner)
        {
            pipelineRunner.Notify(this.ResolveResource).After<BootstrapperContributor>();
        }

        private PipelineContinuation ResolveResource(ICommunicationContext context)
        {
            if (context.PipelineData.SelectedResource == null)
            {
                var uriToMath = context.GetRequestUriRelativeToRoot();
                var uriMatch = this.uriRepository.Match(uriToMath);
                
                if (uriMatch != null)
                {
                    context.PipelineData.SelectedResource = uriMatch;
                    context.PipelineData.ResourceKey = uriMatch.ResourceKey;
                    context.Request.UriName = uriMatch.UriName;
                }
                else
                {
                    context.OperationResult = this.CreateNotFound(context);
                    
                    return PipelineContinuation.RenderNow;
                }
            }
            else
            {
                this.Log.WriteInfo(
                    "Not resolving any resource as a resource with key {0} has already been selected.".With(
                        context.PipelineData.SelectedResource.ResourceKey));
            }

            return PipelineContinuation.Continue;
        }

        private OperationResult.NotFound CreateNotFound(ICommunicationContext context)
        {
            return new OperationResult.NotFound
            {
                Description =
                    "No registered resource could be found for "
                    + context.Request.Uri
            };
        }
    }
}