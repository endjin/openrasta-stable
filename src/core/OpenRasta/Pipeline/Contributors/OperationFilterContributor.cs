namespace OpenRasta.Pipeline.Contributors
{
    using OpenRasta.DI;
    using OpenRasta.OperationModel;

    public class OperationFilterContributor :
        AbstractOperationProcessing<IOperationFilter, KnownStages.IOperationFiltering>,
        KnownStages.IOperationFiltering
    {
        public OperationFilterContributor(IDependencyResolver resolver) : base(resolver)
        {
        }

        protected override void InitializeWhen(IPipelineExecutionOrder pipeline)
        {
            pipeline.After<KnownStages.IOperationCreation>();
        }
    }
}