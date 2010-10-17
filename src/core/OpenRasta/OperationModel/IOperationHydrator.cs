namespace OpenRasta.OperationModel
{
    using OpenRasta.Pipeline;

    public interface IOperationHydrator : IOperationProcessor<KnownStages.IRequestDecoding>
    {
    }
}