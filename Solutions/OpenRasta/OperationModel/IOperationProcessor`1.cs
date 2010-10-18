// ReSharper disable UnusedTypeParameter
namespace OpenRasta.OperationModel
{
    using OpenRasta.Pipeline;

    public interface IOperationProcessor<T> : IOperationProcessor
        where T : IPipelineContributor
    {
    }
}

// ReSharper restore UnusedTypeParameter