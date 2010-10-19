namespace OpenRasta.OperationModel.Interceptors
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using OpenRasta.Contracts.OperationModel;
    using OpenRasta.Contracts.OperationModel.Interceptors;

    #endregion

    public abstract class InterceptorProviderAttribute : Attribute, IOperationInterceptorProvider
    {
        public abstract IEnumerable<IOperationInterceptor> GetInterceptors(IOperation operation);
    }
}