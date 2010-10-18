namespace OpenRasta.OperationModel
{
    using OpenRasta.Pipeline;

    public interface IOperationFilter : IOperationProcessor<KnownStages.IOperationFiltering>
    {
    }
}