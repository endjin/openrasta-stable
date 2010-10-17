namespace OpenRasta.OperationModel.Interceptors
{
    using System.Collections.Generic;

    public interface IOperationInterceptorProvider
    {
        IEnumerable<IOperationInterceptor> GetInterceptors(IOperation operation);
    }
}