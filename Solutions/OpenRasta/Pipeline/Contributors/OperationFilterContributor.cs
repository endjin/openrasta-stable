namespace OpenRasta.Pipeline.Contributors
{
    #region Using Directives

    using OpenRasta.Contracts.DI;
    using OpenRasta.Contracts.OperationModel;
    using OpenRasta.Contracts.Pipeline;

    #endregion

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