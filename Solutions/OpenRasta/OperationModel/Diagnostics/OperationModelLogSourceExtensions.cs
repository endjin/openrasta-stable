namespace OpenRasta.OperationModel.Diagnostics
{
    #region Using Directives

    using System.Collections.Generic;

    using OpenRasta.Contracts.Diagnostics;
    using OpenRasta.Contracts.OperationModel;

    #endregion

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