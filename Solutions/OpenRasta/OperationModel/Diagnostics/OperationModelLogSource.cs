namespace OpenRasta.OperationModel.Diagnostics
{
    using System.Collections.Generic;

    using OpenRasta.Contracts.Diagnostics;
    using OpenRasta.Contracts.OperationModel;
    using OpenRasta.Diagnostics;

    [LogCategory("openrasta.operationmodel")]
    public class OperationModelLogSource : ILogSource
    {
    }

    public static class OperationModelLogSourceExtensions
    {
        public static void NoResourceOrUriName(this ILogger<OperationModelLogSource> log)
        {
            log.WriteDebug("No resource or no uri name. Not filtering.");
        } 
    
        public static void FoundOperations(this ILogger<OperationModelLogSource> log, ICollection<IOperation> operations)
        {
            log.WriteDebug("Found {0} operations with correct attributes", operations.Count);
        }
    }
}
