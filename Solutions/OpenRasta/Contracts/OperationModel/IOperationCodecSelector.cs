namespace OpenRasta.Contracts.OperationModel
{
    using OpenRasta.Pipeline;

    public interface IOperationCodecSelector : IOperationProcessor<KnownStages.ICodecRequestSelection>
    {
    }
}