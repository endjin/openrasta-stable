namespace OpenRasta.OperationModel
{
    using System.Collections.Generic;

    using OpenRasta.TypeSystem;

    public interface IOperationCreator
    {
        IEnumerable<IOperation> CreateOperations(IEnumerable<IType> handlers);
    }
}