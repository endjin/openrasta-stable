namespace OpenRasta.Contracts.OperationModel
{
    using OpenRasta.Pipeline;

    public interface IOperationFilter : IOperationProcessor<KnownStages.IOperationFiltering>
    {
    }
}