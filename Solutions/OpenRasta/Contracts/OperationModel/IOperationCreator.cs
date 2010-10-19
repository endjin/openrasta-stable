namespace OpenRasta.Contracts.OperationModel
{
    using System.Collections.Generic;

    using OpenRasta.Contracts.TypeSystem;

    public interface IOperationCreator
    {
        IEnumerable<IOperation> CreateOperations(IEnumerable<IType> handlers);
    }
}