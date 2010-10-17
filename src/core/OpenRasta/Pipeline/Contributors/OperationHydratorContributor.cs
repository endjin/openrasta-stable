namespace OpenRasta.Pipeline.Contributors
{
    using OpenRasta.DI;
    using OpenRasta.OperationModel;

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