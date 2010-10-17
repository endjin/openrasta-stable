namespace OpenRasta.Security
{
    using System;
    using System.Collections.Generic;

    using OpenRasta.DI;
    using OpenRasta.OperationModel;
    using OpenRasta.OperationModel.Interceptors;

    public class RequiresRoleAttribute : InterceptorProviderAttribute
    {
        private readonly string roleName;

        public RequiresRoleAttribute(string roleName)
        {
            if (roleName == null)
            {
                throw new ArgumentNullException("roleName");
            }

            this.roleName = roleName;
        }

        public override IEnumerable<IOperationInterceptor> GetInterceptors(IOperation operation)
        {
            yield return DependencyManager.GetService<RequiresAuthenticationInterceptor>();
            
            var roleInterceptor = DependencyManager.GetService<RequiresRoleInterceptor>();
            roleInterceptor.Role = this.roleName;
            
            yield return roleInterceptor;
        }
    }
}