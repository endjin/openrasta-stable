// ReSharper disable UnusedTypeParameter
namespace OpenRasta.Contracts.OperationModel
{
    using OpenRasta.Contracts.Pipeline;

    public interface IOperationProcessor<T> : IOperationProcessor
        where T : IPipelineContributor
    {
    }
}

// ReSharper restore UnusedTypeParameter