namespace OpenRasta.Contracts.OperationModel
{
    #region Using Directives

    using System.Collections.Generic;

    using OpenRasta.Contracts.TypeSystem;

    #endregion

    public interface IOperationCreator
    {
        IEnumerable<IOperation> CreateOperations(IEnumerable<IType> handlers);
    }
}