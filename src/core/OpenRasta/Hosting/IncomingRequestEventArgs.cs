namespace OpenRasta.Hosting
{
    using System;

    using OpenRasta.Web;

    public abstract class IncomingRequestEventArgs : EventArgs
    {
        public IncomingRequestEventArgs(ICommunicationContext context)
        {
            this.Context = context;
        }

        public ICommunicationContext Context { get; set; }
    }

    public class IncomingRequestProcessedEventArgs : IncomingRequestEventArgs
    {
        public IncomingRequestProcessedEventArgs(ICommunicationContext context)
            : base(context)
        {
        }
    }

    public class IncomingRequestReceivedEventArgs : IncomingRequestEventArgs
    {
        public IncomingRequestReceivedEventArgs(ICommunicationContext context)
            : base(context)
        {
        }
    }
}