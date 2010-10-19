namespace OpenRasta.Hosting
{
    using OpenRasta.Contracts.Web;

    public class IncomingRequestProcessedEventArgs : IncomingRequestEventArgs
    {
        public IncomingRequestProcessedEventArgs(ICommunicationContext context) : base(context)
        {
        }
    }
}