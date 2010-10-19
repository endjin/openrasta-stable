namespace OpenRasta.Diagnostics
{
    #region Using Directives

    using OpenRasta.Contracts;
    using OpenRasta.Contracts.Web;
    using OpenRasta.Exceptions;

    #endregion

    public class OperationContextErrorCollector : IErrorCollector
    {
        private readonly ICommunicationContext context;

        public OperationContextErrorCollector(ICommunicationContext context)
        {
            this.context = context;
        }

        public void AddServerError(Error error)
        {
            this.context.ServerErrors.Add(error);
        }
    }
}