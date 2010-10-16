namespace OpenRasta.Diagnostics
{
    using OpenRasta.Web;

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