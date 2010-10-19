namespace OpenRasta.Authorization
{
    #region Using Directives

    using OpenRasta.Contracts.OperationModel;
    using OpenRasta.Contracts.Web;
    using OpenRasta.OperationModel.Interceptors;
    using OpenRasta.Web;

    #endregion

    public class RequiresRoleInterceptor : OperationInterceptor
    {
        private readonly ICommunicationContext context;

        public RequiresRoleInterceptor(ICommunicationContext context)
        {
            this.context = context;
        }

        public string Role { get; set; }

        public override bool BeforeExecute(IOperation operation)
        {
            var isAuthorized = this.Role == null || this.context.User.IsInRole(this.Role);
            
            if (!isAuthorized)
            {
                this.context.OperationResult = new OperationResult.Unauthorized();
            }
            
            return isAuthorized;
        }
    }
}