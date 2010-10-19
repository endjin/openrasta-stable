namespace OpenRasta.Pipeline.Contributors
{
    #region Using Directives

    using OpenRasta.Contracts.Authentication;
    using OpenRasta.Contracts.DI;
    using OpenRasta.Contracts.Diagnostics;
    using OpenRasta.Contracts.Pipeline;
    using OpenRasta.Contracts.Web;
    using OpenRasta.Web;

    #endregion

    public class AuthenticationChallengerContributor : IPipelineContributor
    {
        private readonly IDependencyResolver resolver;

        public AuthenticationChallengerContributor(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public ILogger Log { get; set; }

        public void Initialize(IPipeline pipelineRunner)
        {
            pipelineRunner.Notify(this.ChallengeIfUnauthorized)
                .After<KnownStages.IOperationExecution>()
                .And
                .Before<KnownStages.IResponseCoding>();
        }

        private PipelineContinuation ChallengeIfUnauthorized(ICommunicationContext context)
        {
            if (context.OperationResult is OperationResult.Unauthorized)
            {
                var supportedSchemes = this.resolver.ResolveAll<IAuthenticationScheme>();

                foreach (var scheme in supportedSchemes)
                {
                    scheme.Challenge(context.Response);
                }
            }

            return PipelineContinuation.Continue;
        }
    }
}