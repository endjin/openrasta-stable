namespace OpenRasta.OperationModel.Filters
{
    using System.Collections.Generic;
    using System.Linq;

    using OpenRasta.Diagnostics;
    using OpenRasta.OperationModel.Diagnostics;
    using OpenRasta.Web;

    public class UriNameOperationFilter : IOperationFilter
    {
        private readonly ICommunicationContext commContext;

        public UriNameOperationFilter(ICommunicationContext commContext)
        {
            this.commContext = commContext;
            this.Log = NullLogger<OperationModelLogSource>.Instance;
        }

        public ILogger<OperationModelLogSource> Log { get; set; }

        public IEnumerable<IOperation> Process(IEnumerable<IOperation> operations)
        {
            if (this.commContext.PipelineData.SelectedResource == null
                || string.IsNullOrEmpty(this.commContext.PipelineData.SelectedResource.UriName))
            {
                this.Log.NoResourceOrUriName();

                return operations;
            }

            var attribOperations = this.OperationsWithMatchingAttribute(operations).ToList();
            
            this.Log.FoundOperations(attribOperations);
            
            return attribOperations.Count > 0 ? attribOperations : operations;
        }

        private IEnumerable<IOperation> OperationsWithMatchingAttribute(IEnumerable<IOperation> operations)
        {
            return from operation in operations
                   let attribute = operation.FindAttribute<HttpOperationAttribute>()
                   where attribute != null
                         && attribute.MatchesUriName(this.commContext.PipelineData.SelectedResource.UriName)
                   select operation;
        }
    }
}