namespace OpenRasta.Hosting
{
    #region Using Directives

    using System;

    using OpenRasta.Contracts.Web;

    #endregion

    public abstract class IncomingRequestEventArgs : EventArgs
    {
        public IncomingRequestEventArgs(ICommunicationContext context)
        {
            this.Context = context;
        }

        public ICommunicationContext Context { get; set; }
    }
}