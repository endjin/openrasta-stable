namespace OpenRasta.Hosting
{
    using OpenRasta.Contracts.Web;

    public class IncomingRequestReceivedEventArgs : IncomingRequestEventArgs
    {
        public IncomingRequestReceivedEventArgs(ICommunicationContext context) : base(context)
        {
        }
    }
}