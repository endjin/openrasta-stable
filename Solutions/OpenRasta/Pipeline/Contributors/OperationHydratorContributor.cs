namespace OpenRasta.Pipeline.Contributors
{
    #region Using Directives

    using OpenRasta.Contracts.DI;
    using OpenRasta.Contracts.OperationModel;
    using OpenRasta.Contracts.Pipeline;

    #endregion

    public class OperationHydratorContributor :
        AbstractOperationProcessing<IOperationHydrator, KnownStages.IRequestDecoding>,
        KnownStages.IRequestDecoding
    {
        public OperationHydratorContributor(IDependencyResolver resolver) : base(resolver)
        {
        }

        protected override void InitializeWhen(IPipelineExecutionOrder pipeline)
        {
            pipeline.After<KnownStages.ICodecRequestSelection>();
        }
    }
}