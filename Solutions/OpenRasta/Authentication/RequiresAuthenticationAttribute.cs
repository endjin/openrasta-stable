namespace OpenRasta.Authentication
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using OpenRasta.Contracts.OperationModel;
    using OpenRasta.Contracts.OperationModel.Interceptors;
    using OpenRasta.Contracts.Web;
    using OpenRasta.DI;
    using OpenRasta.OperationModel.Interceptors;

    #endregion

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class RequiresAuthenticationAttribute : InterceptorProviderAttribute
    {
        public override IEnumerable<IOperationInterceptor> GetInterceptors(IOperation operation)
        {
            return new[]
            {
                new RequiresAuthenticationInterceptor(DependencyManager.GetService<ICommunicationContext>())
            };
        }
    }
}