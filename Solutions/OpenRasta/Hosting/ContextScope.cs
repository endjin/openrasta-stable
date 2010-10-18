namespace OpenRasta.Hosting
{
    using System;
    using System.Runtime.Remoting.Messaging;

    public class ContextScope : IDisposable
    {
        private readonly object hostContext;
        private readonly object savedHostContext;

        public ContextScope(object context)
        {
            this.savedHostContext = CallContext.HostContext;
            this.hostContext = CallContext.HostContext = context;
        }

        public void Dispose()
        {
            if (this.hostContext != this.savedHostContext)
            {
                CallContext.HostContext = this.savedHostContext;
            }
        }
    }
}