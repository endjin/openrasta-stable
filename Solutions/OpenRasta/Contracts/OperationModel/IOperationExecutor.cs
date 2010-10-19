namespace OpenRasta.Contracts.OperationModel
{
    using System.Collections.Generic;

    using OpenRasta.Web;

    public interface IOperationExecutor
    {
        OperationResult Execute(IEnumerable<IOperation> operations);
    }
}