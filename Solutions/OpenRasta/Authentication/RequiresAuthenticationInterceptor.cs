namespace OpenRasta.Authentication
{
    using OpenRasta.Contracts.OperationModel;
    using OpenRasta.Contracts.Web;
    using OpenRasta.OperationModel.Interceptors;
    using OpenRasta.Web;

    public class RequiresAuthenticationInterceptor : OperationInterceptor
    {
        private readonly ICommunicationContext context;

        public RequiresAuthenticationInterceptor(ICommunicationContext context)
        {
            this.context = context;
        }

        public override bool BeforeExecute(IOperation operation)
        {
            if (this.context.User == null || this.context.User.Identity == null || !this.context.User.Identity.IsAuthenticated)
            {
                this.context.OperationResult = new OperationResult.Unauthorized();
                return false;
            }

            return true;
        }
    }
}