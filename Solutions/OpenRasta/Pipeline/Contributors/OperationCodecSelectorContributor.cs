namespace OpenRasta.Pipeline.Contributors
{
    #region Using Directives

    using OpenRasta.Contracts.DI;
    using OpenRasta.Contracts.OperationModel;
    using OpenRasta.Contracts.Pipeline;
    using OpenRasta.Contracts.Web;
    using OpenRasta.Web;

    #endregion

    public class OperationCodecSelectorContributor
        : AbstractOperationProcessing<IOperationCodecSelector, KnownStages.ICodecRequestSelection>,
          KnownStages.ICodecRequestSelection
    {
        public OperationCodecSelectorContributor(IDependencyResolver resolver) : base(resolver)
        {
        }

        protected override void InitializeWhen(IPipelineExecutionOrder pipeline)
        {
            pipeline.After<KnownStages.IOperationFiltering>();
        }

        protected override PipelineContinuation OnOperationsEmpty(ICommunicationContext context)
        {
            context.OperationResult = new OperationResult.RequestMediaTypeUnsupported();
            
            return PipelineContinuation.RenderNow;
        }
    }
}