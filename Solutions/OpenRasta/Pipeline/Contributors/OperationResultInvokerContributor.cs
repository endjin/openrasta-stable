namespace OpenRasta.Pipeline.Contributors
{
    #region Using Directives

    using OpenRasta.Contracts.Diagnostics;
    using OpenRasta.Contracts.Pipeline;
    using OpenRasta.Contracts.Web;
    using OpenRasta.Extensions;

    #endregion

    public class OperationResultInvokerContributor : KnownStages.IOperationResultInvocation
    {
        public ILogger Log { get; set; }

        public PipelineContinuation RunOperationResult(ICommunicationContext context)
        {
            this.Log.WriteInfo("Executing OperationResult {0}.".With(context.OperationResult));
            
            context.OperationResult.Execute(context);
            context.Response.Entity.Instance = context.OperationResult.ResponseResource;

            return PipelineContinuation.Continue;
        }

        public void Initialize(IPipeline pipelineRunner)
        {
            pipelineRunner.Notify(this.RunOperationResult).After<KnownStages.IOperationExecution>();
        }
    }
}