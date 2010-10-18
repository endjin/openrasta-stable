namespace OpenRasta.OperationModel.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OpenRasta.Diagnostics;
    using OpenRasta.Web;

    public class HttpMethodOperationFilter : IOperationFilter
    {
        private readonly IRequest request;

        public HttpMethodOperationFilter(IRequest request)
        {
            this.request = request;
            this.Log = NullLogger.Instance;
        }

        public ILogger Log { get; set; }

        public IEnumerable<IOperation> Process(IEnumerable<IOperation> operations)
        {
            operations = operations.ToList();
            
            var operationWithMatchingName = this.OperationsWithMatchingName(operations);
            var operationWithMatchingAttribute = this.OperationsWithMatchingAttribute(operations);
            
            this.Log.WriteDebug("Found {0} operation(s) with a matching name.", operationWithMatchingName.Count());
            this.Log.WriteDebug("Found {0} operation(s) with matching [HttpOperation] attribute.", operationWithMatchingAttribute.Count());
            
            return operationWithMatchingName.Union(operationWithMatchingAttribute);
        }

        private IEnumerable<IOperation> OperationsWithMatchingAttribute(IEnumerable<IOperation> operations)
        {
            return from operation in operations
                   let httpAttribute = operation.FindAttribute<HttpOperationAttribute>()
                   where httpAttribute != null && httpAttribute.MatchesHttpMethod(this.request.HttpMethod)
                   select operation;
        }

        private IEnumerable<IOperation> OperationsWithMatchingName(IEnumerable<IOperation> operations)
        {
            return from operation in operations
                   where operation.Name.StartsWith(this.request.HttpMethod, StringComparison.OrdinalIgnoreCase)
                   select operation;
        }
    }
}