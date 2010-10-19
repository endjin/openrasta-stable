namespace OpenRasta.OperationModel
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;

    using OpenRasta.Contracts.OperationModel;
    using OpenRasta.Web;

    #endregion

    public class OperationExecutor : IOperationExecutor
    {
        public OperationResult Execute(IEnumerable<IOperation> operations)
        {
            var operation = operations.FirstOrDefault();
            var result = operation.Invoke().ToList();
            
            if (result.Count == 0)
            {
                return new OperationResult.NoContent();
            }

            var firstResult = result[0];
            var operationResult = firstResult.Value as OperationResult;

            if (operationResult != null)
            {
                return operationResult;
            }

            return new OperationResult.OK(firstResult.Value);
        }
    }
}