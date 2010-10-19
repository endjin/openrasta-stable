namespace OpenRasta.Contracts.OperationModel
{
    using OpenRasta.Pipeline;

    public interface IOperationHydrator : IOperationProcessor<KnownStages.IRequestDecoding>
    {
    }
}