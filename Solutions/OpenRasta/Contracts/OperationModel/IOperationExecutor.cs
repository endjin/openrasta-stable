namespace OpenRasta.Contracts.OperationModel
{
    #region Using Directives

    using System.Collections.Generic;

    using OpenRasta.Web;

    #endregion

    public interface IOperationExecutor
    {
        OperationResult Execute(IEnumerable<IOperation> operations);
    }
}